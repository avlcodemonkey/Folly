namespace Folly.Models;

/// <summary>
/// Subset of AuditLog used only for showing search results.
/// </summary>
public sealed record AuditLogSearchResult {
    public long Id { get; init; }

    public Guid BatchId { get; init; }

    public string Entity { get; init; } = "";

    public string State { get; init; } = "";

    public string UniversalDate { get; init; } = "";

    public string UserFullName { get; init; } = "";
}
