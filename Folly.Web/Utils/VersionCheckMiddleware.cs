using System.Reflection;

namespace Folly.Utils;

/// <summary>
/// Checks that the requested version matches the current app version and adds the refresh header to the response if not.
/// </summary>
public sealed class VersionCheckMiddleware {
    private static string? _VersionNumber;
    private readonly RequestDelegate _Next;

    public VersionCheckMiddleware(RequestDelegate next) {
        _Next = next;
        _VersionNumber = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
    }

    public async Task InvokeAsync(HttpContext context) {
        var acceptVersion = context.Request.Headers[HtmxHeaders.Version].ToString();
        if (!string.IsNullOrWhiteSpace(acceptVersion) && _VersionNumber != acceptVersion) {
            context.Response.Headers[HtmxHeaders.Refresh] = "true";
        }
        await _Next(context);
    }
}
