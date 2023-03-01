using Folly.Configuration;
using Folly.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers
{
    public abstract class BaseController : Controller
    {
        protected const string IDParameter = "id";
        protected IAppConfiguration AppConfig { get; set; }
        protected ILogger<Controller> Logger { get; set; }

        protected int ID { get; set; }
        public const string ErrorProperty = "Error";
        public const string IsPetchRequestProperty = "IsPetchRequest";
        public const string MessageProperty = "Message";
        public const string TitleProperty = "Title";

        public BaseController(IAppConfiguration appConfig, ILogger<Controller> logger)
        {
            AppConfig = appConfig;
            Logger = logger;
        }

        public IActionResult Data(object data) => Ok(data);

        public IActionResult Error(string error)
        {
            if (Request.IsAjaxRequest())
                return Ok(new { error });

            // this shouldn't happen, but its possible so show a safe error page as a fallback
            ViewData[ErrorProperty] = error;
            return View("Error");
        }

        public IActionResult Rows(IEnumerable<object> rows) => Ok(new { Rows = rows });

        public IActionResult Success(string message = "")
        {
            if (!Request.IsAjaxRequest())
            {
                // this shouldn't happen
                ViewData[ErrorProperty] = Core.ErrorGeneric;
                return View("Error");
            }
            return message.IsEmpty() ? Ok(new { result = true }) : Ok(new { message });
        }
    }
}
