namespace Folly.Models;

/// <summary>
/// Represents an action method the user could execute.  Not editable by users, so no model validation needed.
/// </summary>
public sealed record Permission : BaseModel {
    public string ActionName { get; init; } = string.Empty;

    public string ControllerName { get; init; } = string.Empty;
}
