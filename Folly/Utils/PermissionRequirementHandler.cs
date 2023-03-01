using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Folly.Utils;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        Endpoint endpoint = null;
        if (context.Resource is HttpContext httpContext)
        {
            endpoint = httpContext.GetEndpoint();
        }
        else if (context.Resource is Endpoint endpoint2)
        {
            endpoint = endpoint2;
        }

        if (endpoint != null)
        {
            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            var parentActionAttributes = actionDescriptor?.MethodInfo.GetCustomAttributes(typeof(ParentActionAttribute), false).Cast<ParentActionAttribute>().Where(x => !x.Action.IsEmpty());

            if (parentActionAttributes.Any())
            {
                parentActionAttributes.SelectMany(x => x.Action.Split(',')).Where(x => !x.IsEmpty()).Each(action => {
                    if (context.User.IsInRole($"{actionDescriptor.ControllerName}.{action}".ToLower()))
                    {
                        context.Succeed(requirement);
                    }
                });
            }
            else if (context.User.IsInRole($"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}".ToLower()))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }

    public const string PolicyName = "HasPermission";
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement()
    { }
}
