using Folly.Constants;

namespace Folly.Models;

public sealed record Alert(
    string Content = "",
    AlertType AlertType = AlertType.Success,
    bool CanDismiss = true
);
