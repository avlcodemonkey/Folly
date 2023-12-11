using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

public abstract class BaseController : Controller {
    protected ILogger<Controller> Logger { get; set; }

    public const string ErrorProperty = "Error";
    public const string MessageProperty = "Message";
    public const string TitleProperty = "Title";

    public BaseController(ILogger<Controller> logger) => Logger = logger;
}
