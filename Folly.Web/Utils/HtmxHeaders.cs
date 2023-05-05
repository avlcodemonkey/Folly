namespace Folly.Utils;

/// <summary>
/// Custom headers to use with HTMX.
/// </summary>
public class HtmxHeaders {
    /// <summary>
    /// Request headers
    /// </summary>
    public const string Request = "hx-request";
    // @todo get this working
    public const string RestoreRequest = "hx-history-restore-request";

    /// <summary>
    /// Response headers
    /// </summary>
    public const string PushUrl = "hx-push-url";
    public const string ReplaceUrl = "hx-push-url";
}
