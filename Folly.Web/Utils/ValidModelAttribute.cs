using Folly.Controllers;
using Folly.Models;
using Folly.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Folly.Utils;

public sealed class ValidModelAttribute : ActionFilterAttribute {
    private readonly bool UseTempData;

    public ValidModelAttribute(bool useTempData = false) => UseTempData = useTempData;

    public override void OnActionExecuting(ActionExecutingContext context) {
        var param = context.ActionArguments.FirstOrDefault(p => p.Value is BaseModel);
        if (param.Value == null)
            context.ModelState.AddModelError("general", Core.ErrorGeneric);

        if (!context.ModelState.IsValid) {
            var controller = (Controller)context.Controller;
            if (UseTempData)
                controller.TempData[BaseController.ErrorProperty] = context.ModelState.ToErrorString();
            else
                controller.ViewData[BaseController.ErrorProperty] = context.ModelState.ToErrorString();
        }
    }
}
