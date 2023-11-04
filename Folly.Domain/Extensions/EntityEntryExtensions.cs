using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Folly.Domain.Extensions;

public static class EntityEntryExtensions {
    public static long? GetPrimaryKey(this EntityEntry entry) {
        if (long.TryParse(entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "", out var primaryKey)) {
            return primaryKey;
        }
        return null;
    }
}
