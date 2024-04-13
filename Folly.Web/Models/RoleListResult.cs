namespace Folly.Models;

/// <summary>
/// Subset of Role used only for showing the role list.
/// </summary>
public sealed record RoleListResult : IAuditedModel {
    public int Id { get; init; }

    public string Name { get; init; } = "";
}
