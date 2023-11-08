using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Folly.Domain.Extensions;

public static class EntityEntryExtensions {
    public static int? GetPrimaryKey(this EntityEntry entry) {
        if (int.TryParse(entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "", out var primaryKey)) {
            return primaryKey;
        }
        return null;
    }
}
