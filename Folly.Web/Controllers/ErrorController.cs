using Folly.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public class ErrorController(ILogger<ErrorController> logger) : BaseController(logger) {
    public IActionResult Index(string? code = null) {
        if (!string.IsNullOrWhiteSpace(code)) {
            Logger.LogError(Core.UnhandledError, code, HttpContext.Features.Get<IStatusCodeReExecuteFeature>()?.OriginalPath);
        }

        return View("Error");
    }
}
