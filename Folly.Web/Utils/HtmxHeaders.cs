namespace Folly.Utils;

/// <summary>
/// Custom headers to use with HTMX.
/// </summary>
public class HtmxHeaders {
    /// <summary>
    /// Request header that is always true when HTMX initiates the request.
    /// </summary>
    public const string Request = "hx-request";

    /// <summary>
    /// Request header that is true when HTMX tried to go back and restore a page.
    /// </summary>
    public const string RestoreRequest = "hx-history-restore-request";

    /// <summary>
    /// Request header that specifies the expected version of the app.
    /// </summary>
    public const string Version = "hx-version";

    /// <summary>
    /// Response header that pushes a new url into the history stack.
    /// </summary>
    public const string PushUrl = "hx-push-url";

    /// <summary>
    /// Response header that replaces the current URL in the location bar.
    /// </summary>
    public const string ReplaceUrl = "hx-push-url";

    /// <summary>
    /// Response header that will do a full refresh of the page client side if set to true.
    /// </summary>
    public const string Refresh = "hx-refresh";
}
