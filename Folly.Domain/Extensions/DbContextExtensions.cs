using System.Text.Json;
using Folly.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Folly.Domain.Extensions;

public static class DbContextExtensions {
    private static readonly List<string> _UnauditedProperties = typeof(AuditableEntity).GetProperties().Select(x => x.Name).ToList();

    public static int? GetPrimaryKey(this EntityEntry entry) {
        if (int.TryParse(entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "", out var primaryKey)) {
            return primaryKey;
        }
        return null;
    }

    public static string ToAuditJson(this IEnumerable<PropertyEntry> properties, bool currentValues = true)
        => JsonSerializer.Serialize(properties.Where(x => !_UnauditedProperties.Contains(x.Metadata.Name))
            .ToDictionary(x => x.Metadata.Name, x => (currentValues ? x.CurrentValue : x.OriginalValue)?.ToString()));
}
