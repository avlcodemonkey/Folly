using Folly.Constants;

namespace Folly.Models;

public sealed record Alert {
    public string Content { get; init; } = "";
    public AlertType AlertType { get; init; } = AlertType.Success;
    public bool CanDismiss { get; init; } = true;
}
