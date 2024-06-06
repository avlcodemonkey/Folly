using Microsoft.EntityFrameworkCore;

namespace Folly.Models;

/// <summary>
/// Represents an audit log entry for tracking change history. Only used for display purposes, so no model validation needed.
/// </summary>
/// <remarks>No need for for validation or display attributes for this model.</remarks>
public sealed record AuditLog(
    long Id,
    Guid BatchId,
    long PrimaryKey,
    EntityState State,
    DateTime Date,
    string UserLastName = "",
    string UserFirstName = "",
    string Entity = "",
    string OldValues = "",
    string NewValues = ""
);
