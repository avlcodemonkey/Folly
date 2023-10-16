using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Folly.Domain.Configurations;

internal class LanguageConfiguration : IEntityTypeConfiguration<Language> {
    void IEntityTypeConfiguration<Language>.Configure(EntityTypeBuilder<Language> builder) {
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(current_timestamp)");
    }
}
