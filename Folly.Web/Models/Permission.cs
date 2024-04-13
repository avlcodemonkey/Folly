namespace Folly.Models;

/// <summary>
/// Represents an action method the user could execute.  Not editable by users, so no model validation needed.
/// </summary>
public sealed record Permission : IAuditedModel {
    public int Id { get; init; }

    public string ActionName { get; init; } = "";

    public string ControllerName { get; init; } = "";
}
