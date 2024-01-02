using Folly.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public abstract class BaseController(ILogger<Controller> logger) : Controller {
    protected ILogger<Controller> Logger { get; set; } = logger;

    public const string ErrorProperty = "Error";
    public const string MessageProperty = "Message";
    public const string TitleProperty = "Title";

}
