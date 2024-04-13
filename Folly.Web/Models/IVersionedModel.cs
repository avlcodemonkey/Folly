namespace Folly.Models;

/// <summary>
/// Fields needed for optimistic concurrency.
/// </summary>
public interface IVersionedModel {
    /// <summary>
    /// Database incremented value for tracking the version of the model.
    /// </summary>
    int RowVersion { get; init; }
}
