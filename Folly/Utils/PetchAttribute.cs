using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Folly.Controllers;

namespace Folly.Utils;

public sealed class PetchAttribute : ActionFilterAttribute
{
    public PetchAttribute(bool isPetch = true) => IsPetch = isPetch;

    public bool IsPetch { get; set; }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (!IsPetch)
            return;
        if (!((Controller)context.Controller).ViewData[BaseController.IsPetchRequestProperty].ToString().ToBool())
            return;
        if (!context.HttpContext.Response.Headers.ContainsKey(PetchConstants.PetchVersion))
            context.HttpContext.Response.Headers.Add(PetchConstants.PetchVersion, PetchConstants.PetchVersionValue);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!IsPetch)
            return;
        var petchController = (Controller)context.Controller;
        var petch = context.HttpContext.Request.Headers[PetchConstants.PetchHeader];
        petchController.ViewData[BaseController.IsPetchRequestProperty] = bool.TryParse(petch, out var isPetchRequest) && isPetchRequest;
    }
}
