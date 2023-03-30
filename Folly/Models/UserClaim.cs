namespace Folly.Models;

public sealed record UserClaim : BaseModel {
    public int UserId { get; init; }

    public string ActionName { get; init; } = "";

    public string ControllerName { get; init; } = "";
}
