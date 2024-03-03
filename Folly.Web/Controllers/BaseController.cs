using Folly.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public abstract class BaseController(ILogger<Controller> logger) : Controller {
    protected ILogger<Controller> Logger { get; set; } = logger;

    /// <summary>
    /// Adds a PushUrl header that the client pushes into browser history.
    /// </summary>
    /// <param name="url"></param>
    public void PushUrl(string? url, HttpMethod? method = null) {
        if (!string.IsNullOrWhiteSpace(url)) {
            Response.Headers.Append(PJax.PushUrl, url);
            Response.Headers.Append(PJax.PushMethod, (method ?? HttpMethod.Get).ToString());
        }
    }
}
