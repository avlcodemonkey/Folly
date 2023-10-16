using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Folly.Domain.Configurations;

internal class PermissionConfiguration : IEntityTypeConfiguration<Permission> {
    void IEntityTypeConfiguration<Permission>.Configure(EntityTypeBuilder<Permission> builder) {
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(current_timestamp)");
    }
}
