using System.Text.Json;
using Folly.Domain.Extensions;
using Folly.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Folly.Domain;

public partial class FollyDbContext : DbContext {
    private readonly string _ConnectionString = "Data Source = ..\\AppData\\Folly.db;";
    private readonly IConfiguration? _Configuration;
    private readonly IHttpContextAccessor? _HttpContextAccessor;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured) {
            return;
        }

        // fall back to using hardcode database path when configuration isn't injected.  this happens when using dotnet ef tools locally
        var connectionString = _Configuration == null ? _ConnectionString : $"Data Source = {_Configuration.GetSection("App").GetSection("Database")["FilePath"]};";

        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Seed();

    public FollyDbContext() { }

    public FollyDbContext(DbContextOptions<FollyDbContext> options, IConfiguration? configuration = null, IHttpContextAccessor? httpContextAccessor = null)
        : base(options) {
        _Configuration = configuration;
        _HttpContextAccessor = httpContextAccessor;
    }

    public DbSet<AuditLog> AuditLog { get; set; }

    public DbSet<Language> Languages { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var changedEntities = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity &&
            (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)).ToList();

        // create change log records based on the current changed entities
        // SaveChanges clears these, so gotta do this first
        var changeLogs = await CreateAuditLogsAsync(changedEntities, cancellationToken);

        // save entity changes to the db
        var result = await base.SaveChangesAsync(cancellationToken);
        if (result > 0) {
            // now save the audit logs, including updating primary keys for added entities
            await SaveAuditLogsAsync(changedEntities, changeLogs, cancellationToken);
        }

        return result;
    }

    private async Task<List<AuditLog>> CreateAuditLogsAsync(List<EntityEntry> changedEntries, CancellationToken cancellationToken) {
        var changeLogs = new List<AuditLog>();

        if (!changedEntries.Any()) {
            return changeLogs;
        }

        var batchId = Guid.NewGuid();
        var userName = _HttpContextAccessor?.HttpContext?.User.Identity?.Name;
        var user = string.IsNullOrEmpty(userName) ? null : await Users.FirstAsync(x => x.UserName == userName, cancellationToken: cancellationToken);

        foreach (var entry in changedEntries) {
            var primaryKey = entry.GetPrimaryKey();
            if (!primaryKey.HasValue) {
                // if we can't parse out a primaryKey, then logging changes is useless so move on
                continue;
            }

            ((AuditableEntity)entry.Entity).TemporaryId = primaryKey.Value;

            var entityName = entry.Entity.GetType().Name;
            var changeLog = new AuditLog {
                BatchId = batchId,
                Entity = entityName,
                PrimaryKey = primaryKey.Value,
                UserId = user?.Id,
                State = entry.State,
                Date = DateTime.UtcNow
            };

            if (entry.State is EntityState.Modified) {
                var changedProperties = entry.Properties.Where(x => x.IsModified).ToList();
                var oldValues = changedProperties.ToDictionary(x => x.Metadata.Name, x => x.OriginalValue?.ToString());
                var newValues = changedProperties.ToDictionary(x => x.Metadata.Name, x => x.CurrentValue?.ToString());

                // switch this to json
                changeLog.OldValues = JsonSerializer.Serialize(oldValues);
                changeLog.NewValues = JsonSerializer.Serialize(newValues);
            }

            changeLogs.Add(changeLog);
        }

        return changeLogs;
    }

    private async Task SaveAuditLogsAsync(List<EntityEntry> changedEntities, List<AuditLog> changeLogs, CancellationToken cancellationToken) {
        if (!changeLogs.Any()) {
            return;
        }

        var keyMap = changedEntities.ToDictionary(GetEntryIdentifier, GetEntryPrimaryKey);

        foreach (var changeLog in changeLogs) {
            if (keyMap.TryGetValue($"{changeLog.Entity}_{changeLog.PrimaryKey}", out var primaryKey)) {
                changeLog.PrimaryKey = primaryKey;
            }
        }

        await AuditLog.AddRangeAsync(changeLogs, cancellationToken);
        await base.SaveChangesAsync(cancellationToken);
    }

    private string GetEntryIdentifier(EntityEntry entry) => $"{entry.Entity.GetType().Name}_{((AuditableEntity)entry.Entity).TemporaryId}";

    private long GetEntryPrimaryKey(EntityEntry entry) => entry.GetPrimaryKey() ?? -1;
}
