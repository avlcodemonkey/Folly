namespace Folly.Constants;

/// <summary>
/// Custom headers to use with pjax.
/// </summary>
public class PJax {
    /// <summary>
    /// Request header that is always true when HTMX initiates the request.
    /// </summary>
    public const string Request = "x-pjax";

    /// <summary>
    /// Request header that specifies the expected version of the app.
    /// </summary>
    public const string Version = "x-pjax-version";

    /// <summary>
    /// Response header that specifies the new page title.
    /// </summary>
    public const string Title = "x-pjax-title";

    /// <summary>
    /// Response header that pushes a new url into the history stack.
    /// </summary>
    public const string PushUrl = "x-pjax-push-url";

    /// <summary>
    /// Response header that pushes a new request method into the history stack.
    /// </summary>
    public const string PushMethod = "x-pjax-push-method";

    /// <summary>
    /// Response header that will do a full refresh of the page client side if set to true.
    /// </summary>
    public const string Refresh = "x-pjax-refresh";
}
