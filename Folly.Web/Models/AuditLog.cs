using Folly.Utils;
using Microsoft.EntityFrameworkCore;

namespace Folly.Models;

/// <summary>
/// Represents an audit log entry for tracking change history. Only used for display purposes, so no model validation needed.
/// </summary>
public sealed record AuditLog : BaseModel {
    public new long Id { get; set; }

    // @todo add Display attributes to use when showing these
    public Guid BatchId { get; set; }

    public string Entity { get; set; } = null!;

    public long PrimaryKey { get; set; }

    /// <summary>
    /// Should only be one of: Deleted = 2, Modified = 3, Added = 4
    /// </summary>
    public EntityState State { get; set; }

    /// <summary>
    /// Descriptive value for the EntityState.
    /// </summary>
    public string? StateDesc => State.ToString();

    public DateTime Date { get; set; }

    public string UniversalDate => Date.ToString("u");

    public int? UserId { get; set; }

    public string? UserLastName { get; set; }

    public string? UserFirstName { get; set; }

    /// <summary>
    /// User name formatted as "last, first".
    /// </summary>
    public string? UserFullName => NameHelper.DisplayName(UserFirstName, UserLastName);

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}
