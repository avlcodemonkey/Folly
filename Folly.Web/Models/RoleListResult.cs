namespace Folly.Models;

/// <summary>
/// Subset of Role used only for showing the role list.
/// </summary>
public sealed record RoleListResult(
    int Id,
    string Name = ""
) : IAuditedModel;
