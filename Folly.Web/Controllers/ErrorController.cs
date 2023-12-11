using Folly.Models;
using Folly.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public class ErrorController : BaseController {

    public ErrorController(ILogger<ErrorController> logger) : base(logger) { }

    public IActionResult Index(string? code = null) {
        if (!string.IsNullOrWhiteSpace(code)) {
            Logger.LogError(Core.UnhandledError, code, HttpContext.Features.Get<IStatusCodeReExecuteFeature>()?.OriginalPath);
        }

        return View("Error");
    }

    [HttpPost]
    public IActionResult LogJavascriptError(JavascriptError error) {
        Logger.LogError(new JavaScriptException(error.Message), Core.JavascriptException);
        return Ok();
    }
}
