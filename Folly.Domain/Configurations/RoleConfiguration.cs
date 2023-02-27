using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Folly.Domain.Models;

namespace Folly.Domain.Migrations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    void IEntityTypeConfiguration<Role>.Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(current_timestamp)");
    }
}
