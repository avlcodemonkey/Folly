using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Folly.Utils;

public sealed class AjaxRequestOnlyAttribute : ActionMethodSelectorAttribute {
    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action) => routeContext.HttpContext.Request.IsAjaxRequest();
}
