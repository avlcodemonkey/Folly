using Folly.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Folly.Domain.Models;

public partial class FollyDbContext : DbContext {
    private const string DatabasePath = "Data Source = ..\\AppData\\Folly.db;";
    private readonly IConfiguration? Configuration;
    private readonly IHttpContextAccessor? HttpContextAccessor;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured)
            return;

        // fall back to using hardcode database path when configuration isn't injected.  this happens when using dotnet ef tools locally
        var connectionString = Configuration == null ? DatabasePath : $"Data Source = {Configuration.GetSection("App").GetSection("Database")["FilePath"]};";

        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Seed();

    public FollyDbContext() { }

    public FollyDbContext(DbContextOptions<FollyDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(options) {
        Configuration = configuration;
        HttpContextAccessor = httpContextAccessor;
    }

    public DbSet<Language> Languages { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var userName = HttpContextAccessor?.HttpContext?.User.Identity?.Name;
        var user = string.IsNullOrEmpty(userName) ? null : await Users.FirstAsync(x => x.UserName == userName, cancellationToken: cancellationToken);

        var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
        foreach (var entity in entries) {
            var model = (BaseEntity)entity.Entity;
            model.UpdatedDate = DateTime.UtcNow;
            model.UpdatedUserId = user?.Id;

            if (entity.State == EntityState.Added) {
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedUserId = user?.Id;
            } else {
                // don't overwrite existing created valus
                entity.Property(nameof(BaseEntity.CreatedDate)).IsModified = false;
                entity.Property(nameof(BaseEntity.CreatedUserId)).IsModified = false;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
