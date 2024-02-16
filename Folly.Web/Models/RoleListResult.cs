namespace Folly.Models;

/// <summary>
/// Subset of Role used only for showing the role list.
/// </summary>
public sealed record RoleListResult : BaseModel {
    public string Name { get; init; } = "";
}
