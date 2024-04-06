namespace Folly.Models;

/// <summary>
/// Fields needed for concurrency with versioned entities.
/// </summary>
public interface IVersionedModel {
    /// <summary>
    /// Database incremented value for tracking the version of the entity.
    /// </summary>
    int RowVersion { get; init; }
}
