namespace Folly.Models;

public record AlpineAlert
{
    public string Content { get; init; } = "";
    public AlertType AlertType { get; init; } = AlertType.Success;
    public bool CanDismiss { get; init; } = true;
}
