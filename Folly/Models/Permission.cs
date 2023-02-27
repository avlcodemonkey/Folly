namespace Folly.Models;

/// <summary>
/// Represents an action method the user could execute.  Not editable by users, so no model validation needed.
/// </summary>
public record Permission : BaseModel
{
    public string ActionName { get; init; }

    public string ControllerName { get; init; }
}
