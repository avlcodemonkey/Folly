using Folly.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public abstract class BaseController(ILogger<Controller> logger) : Controller {
    protected ILogger<Controller> Logger { get; set; } = logger;

    public void PushAction(string action) {
        if (!string.IsNullOrWhiteSpace(action)) {
            Response.Headers.Append(HtmxHeaders.PushUrl, Url.Action(action));
        }
    }
}
