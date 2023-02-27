﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Folly.Domain.Models;

namespace Folly.Domain.Migrations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("(current_timestamp)");
        builder.Property(e => e.Status).HasDefaultValueSql("(1)");
    }
}
