namespace Folly.Models;

/// <summary>
/// Subset of User used only for showing the user list.
/// </summary>
public sealed record UserListResult(
    int Id,
    string UserName = "",
    string FirstName = "",
    string LastName = "",
    string Email = ""
) : IAuditedModel;
