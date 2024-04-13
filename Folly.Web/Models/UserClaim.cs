namespace Folly.Models;

/// <summary>
/// Represents a controller.action that the user can access. Only used for authorization, so no model validation needed.
/// </summary>
public sealed record UserClaim : IAuditedModel {
    public int Id { get; init; }

    public int UserId { get; init; }

    public string ActionName { get; init; } = "";

    public string ControllerName { get; init; } = "";
}
