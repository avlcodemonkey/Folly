namespace Folly.Domain.Models;

/// <summary>
/// Fields needed for concurrency with versioned entities.
/// </summary>
public interface IVersionedEntity {
    /// <summary>
    /// Database incremented value for tracking the version of the entity.
    /// </summary>
    int RowVersion { get; set; }
}
