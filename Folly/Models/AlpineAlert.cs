namespace Folly.Models;

public record AlpineAlert
{
    public string Content { get; init; } = "";
    public LitAlertType AlertType { get; init; } = LitAlertType.Success;
}
