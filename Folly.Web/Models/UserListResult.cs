namespace Folly.Models;

/// <summary>
/// Subset of User used only for showing the user list.
/// </summary>
public sealed record UserListResult : BaseModel {
    public string UserName { get; init; } = "";

    public string FirstName { get; init; } = "";

    public string LastName { get; init; } = "";

    public string Email { get; init; } = "";
}
