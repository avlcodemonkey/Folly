namespace Folly.Models;

public sealed record UserClaim : BaseModel {
    public int UserId { get; init; }

    public string ActionName { get; init; } = string.Empty;

    public string ControllerName { get; init; } = string.Empty;
}
