using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Folly.Configuration;
using Folly.Models;
using Folly.Resources;

namespace Folly.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IAppConfiguration appConfig) : base(appConfig)
        {
        }

        public IActionResult Index(string code = null)
        {
            if (!code.IsEmpty())
                Serilog.Log.Error(Core.UnhandledError, code, HttpContext.Features.Get<IStatusCodeReExecuteFeature>()?.OriginalPath);
            return View("Error");
        }

        [HttpPost]
        public IActionResult LogJavascriptError(JavascriptError error)
        {
            Serilog.Log.Error(new JavaScriptException(error.Message), Core.JavascriptException);
            return Ok();
        }
    }
}
