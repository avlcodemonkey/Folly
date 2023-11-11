using Folly.Domain.Extensions;
using Folly.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Folly.Domain;

public sealed class FollyDbContext : DbContext {
    private readonly string _ConnectionString = "Data Source = ..\\AppData\\Folly.db;";
    private readonly IConfiguration? _Configuration;
    private readonly IHttpContextAccessor? _HttpContextAccessor;

    public DbSet<AuditLog> AuditLog { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }

    public FollyDbContext() { }

    public FollyDbContext(DbContextOptions<FollyDbContext> options, IConfiguration? configuration = null, IHttpContextAccessor? httpContextAccessor = null)
        : base(options) {
        _Configuration = configuration;
        _HttpContextAccessor = httpContextAccessor;

        // don't create savepoints when SaveChanges is called inside a transaction. savepoints can cause database locking issues
        Database.AutoSavepointsEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured) {
            return;
        }

        // fall back to using hardcoded database path when configuration isn't injected.  this happens when using dotnet ef tools locally
        var connectionString = _Configuration == null ? _ConnectionString : $"Data Source = {_Configuration.GetSection("App").GetSection("Database")["FilePath"]};";

        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyDefaults().Seed();

    /// <summary>
    /// Add custom logic for auditing to the base SaveChangesAsync method.
    /// </summary>
    /// <remarks>App code should always use SaveChangesAsync instead of SaveChanges. SaveChanges is not overridden for use in tests.</remarks>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var changedEntities = ChangeTracker.Entries().Where(x => x.Entity is AuditableEntity &&
            (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)).ToList();

        // create audit log records based on the current changed entities
        // SaveChanges resets the change tracker so gotta do this first
        var auditLogs = await CreateAuditLogsAsync(changedEntities, cancellationToken);

        // set the created/updated date fields on these entities for easy access
        changedEntities.ForEach(x => {
            var model = (AuditableEntity)x.Entity;
            model.UpdatedDate = DateTime.UtcNow;

            if (x.State == EntityState.Added) {
                model.CreatedDate = DateTime.UtcNow;
            } else {
                // don't overwrite existing createdDate values
                x.Property(nameof(AuditableEntity.CreatedDate)).IsModified = false;
            }
        });

        using var transaction = await Database.BeginTransactionAsync(cancellationToken);

        try {
            // save entity changes to the db
            var result = await base.SaveChangesAsync(cancellationToken);
            if (result > 0) {
                // now save the audit logs, including updating primary keys for added entities
                await SaveAuditLogsAsync(changedEntities, auditLogs, cancellationToken);
            }
            await transaction.CommitAsync(cancellationToken);
            return result;
        } catch (Exception) {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<List<AuditLog>> CreateAuditLogsAsync(List<EntityEntry> changedEntries, CancellationToken cancellationToken) {
        var auditLogs = new List<AuditLog>();

        if (!changedEntries.Any()) {
            return auditLogs;
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

            // track the temporaryId that EF assigns so we can update our auditLogs with the database assigned ID after saving to the db
            ((AuditableEntity)entry.Entity).TemporaryId = primaryKey.Value;

            var entityName = entry.Entity.GetType().Name;
            var auditLog = new AuditLog {
                BatchId = batchId,
                Entity = entityName,
                PrimaryKey = primaryKey.Value,
                UserId = user?.Id,
                State = entry.State,
                Date = DateTime.UtcNow
            };

            if (entry.State is EntityState.Added) {
                auditLog.NewValues = entry.Properties.ToAuditJson(true);
            } else if (entry.State is EntityState.Deleted) {
                auditLog.OldValues = entry.Properties.ToAuditJson(false);
            } else if (entry.State is EntityState.Modified) {
                var changedProperties = entry.Properties.Where(x => x.IsModified && x.OriginalValue?.ToString() != x.CurrentValue?.ToString()).ToList();
                if (changedProperties.Any()) {
                    auditLog.OldValues = changedProperties.ToAuditJson(false);
                    auditLog.NewValues = changedProperties.ToAuditJson(true);
                } else {
                    // EF thinks this object changed, but properties are all the same so don't add an auditLog record
                    continue;
                }
            }

            auditLogs.Add(auditLog);
        }

        return auditLogs;
    }

    private async Task SaveAuditLogsAsync(List<EntityEntry> changedEntities, List<AuditLog> auditLogs, CancellationToken cancellationToken) {
        if (!auditLogs.Any()) {
            return;
        }

        // create a dictionary for the added entities so we can set the new primaryKey on the audit log record
        var keyMap = changedEntities.ToDictionary(GetEntryIdentifier, GetEntryPrimaryKey);

        var addedEntityLogs = auditLogs.Where(x => x.State == EntityState.Added);
        foreach (var auditLog in addedEntityLogs) {
            if (keyMap.TryGetValue($"{auditLog.Entity}_{auditLog.PrimaryKey}", out var primaryKey)) {
                auditLog.PrimaryKey = primaryKey;
            }
        }

        await AuditLog.AddRangeAsync(auditLogs, cancellationToken);
        await base.SaveChangesAsync(cancellationToken);
    }

    private string GetEntryIdentifier(EntityEntry entry) => $"{entry.Entity.GetType().Name}_{((AuditableEntity)entry.Entity).TemporaryId}";

    private int GetEntryPrimaryKey(EntityEntry entry) => entry.GetPrimaryKey() ?? -1;
}
