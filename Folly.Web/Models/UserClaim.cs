namespace Folly.Models;

/// <summary>
/// Represents a controller.action that the user can access. Only used for authorization, so no model validation needed.
/// </summary>
public sealed record UserClaim(
    int Id,
    int UserId,
    string ControllerName = "",
    string ActionName = ""
) : IAuditedModel;

