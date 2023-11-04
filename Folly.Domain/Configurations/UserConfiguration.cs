using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Folly.Domain.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User> {
    void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder) => builder.Property(e => e.Status).HasDefaultValueSql("(1)");
}
