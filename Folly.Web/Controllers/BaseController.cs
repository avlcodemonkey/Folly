using Folly.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public abstract class BaseController(ILogger<Controller> logger) : Controller {
    protected ILogger<Controller> Logger { get; set; } = logger;

    /// <summary>
    /// Adds a PushUrl header that the client pushes into browser history.
    /// </summary>
    /// <param name="url"></param>
    public void PushUrl(string? url) {
        if (!string.IsNullOrWhiteSpace(url)) {
            Response.Headers.Append(HtmxHeaders.PushUrl, url);
        }
    }
}
