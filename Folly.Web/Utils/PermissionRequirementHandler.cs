using System.Globalization;
using Folly.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Folly.Utils;

public sealed class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement) {
        Endpoint? endpoint = null;
        if (context.Resource is HttpContext httpContext) {
            endpoint = httpContext.GetEndpoint();
        } else if (context.Resource is Endpoint endpoint2) {
            endpoint = endpoint2;
        }

        if (endpoint != null) {
            var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            var parentActions = descriptor?.MethodInfo.GetCustomAttributes(typeof(ParentActionAttribute), false)
                .Cast<ParentActionAttribute>().Where(x => !string.IsNullOrWhiteSpace(x.Action));

            if (parentActions?.SelectMany(x => x.Action.Split(',')).Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"{descriptor?.ControllerName}.{x}".ToLower(CultureInfo.InvariantCulture)).Any(context.User.IsInRole) == true
            ) {
                context.Succeed(requirement);
            } else if (context.User.IsInRole($"{descriptor?.ControllerName}.{descriptor?.ActionName}".ToLower(CultureInfo.InvariantCulture))) {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }

    public const string PolicyName = "HasPermission";
}

public sealed class PermissionRequirement : IAuthorizationRequirement {
    public PermissionRequirement() { }
}
