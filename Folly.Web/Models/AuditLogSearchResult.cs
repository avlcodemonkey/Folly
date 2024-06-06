namespace Folly.Models;

/// <summary>
/// Subset of AuditLog used only for showing search results.
/// </summary>
public sealed record AuditLogSearchResult(
    long Id,
    Guid BatchId,
    string Entity = "",
    string State = "",
    string UniversalDate = "",
    string UserFullName = ""
);
