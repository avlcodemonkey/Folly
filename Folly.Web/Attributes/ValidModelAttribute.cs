using Folly.Controllers;
using Folly.Extensions;
using Folly.Models;
using Folly.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Folly.Attributes;

public sealed class ValidModelAttribute : ActionFilterAttribute {
    private readonly bool _UseTempData;

    public ValidModelAttribute(bool useTempData = false) => _UseTempData = useTempData;

    public override void OnActionExecuting(ActionExecutingContext context) {
        var param = context.ActionArguments.FirstOrDefault(p => p.Value is BaseModel);
        if (param.Value == null)
            context.ModelState.AddModelError("general", Core.ErrorGeneric);

        if (!context.ModelState.IsValid) {
            var controller = (Controller)context.Controller;
            if (_UseTempData)
                controller.TempData[BaseController.ErrorProperty] = context.ModelState.ToErrorString();
            else
                controller.ViewData[BaseController.ErrorProperty] = context.ModelState.ToErrorString();
        }
    }
}