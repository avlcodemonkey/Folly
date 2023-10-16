using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Folly.Domain.Configurations;

internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission> {
    void IEntityTypeConfiguration<RolePermission>.Configure(EntityTypeBuilder<RolePermission> builder) {
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(current_timestamp)");
    }
}
