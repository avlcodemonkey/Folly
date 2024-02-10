namespace Folly.Models;

/// <summary>
/// Subset of AuditLog used only for showing search results.
/// </summary>
public sealed record AuditLogSearchResult : BaseModel {
    public new long Id { get; init; }

    // @todo add Display attributes to use when showing these
    public Guid BatchId { get; init; }

    public string Entity { get; init; } = "";

    /// <summary>
    /// Descriptive value for the EntityState.
    /// </summary>
    public string State { get; init; } = "";

    public string UniversalDate { get; init; } = "";

    public string UserFullName { get; init; } = "";
}
