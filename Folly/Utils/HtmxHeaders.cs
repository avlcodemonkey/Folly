namespace Folly.Utils
{
    /// <summary>
    /// Custom headers to use with HTMX.
    /// </summary>
    public class HtmxHeaders
    {
        /// <summary>
        /// Request headers
        /// </summary>
        public const string Request = "hx-request";

        /// <summary>
        /// Response headers
        /// </summary>
        public const string PushUrl = "hx-push-url";
        public const string ReplaceUrl = "hx-push-url";
    }
}
