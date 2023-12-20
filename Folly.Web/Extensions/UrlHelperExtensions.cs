using Folly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Extensions;

public static class UrlHelperExtensions {
    /// <summary>
    /// Builds a URL for an action removing Id from route data and appending the id placeholder for mustache to use.
    /// </summary>
    public static string ActionForMustache(this IUrlHelper urlHelper, string actionName) {
        var id = nameof(BaseModel.Id).ToLower();
        var routeParams = new Dictionary<string, string> { { id, string.Empty } };
        return $"{urlHelper.Action(actionName, routeParams)}/{{{{{id}}}}}";
    }
}
