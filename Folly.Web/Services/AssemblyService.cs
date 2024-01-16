using System.Globalization;
using System.Reflection;
using Folly.Attributes;
using Folly.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Services;

public sealed class AssemblyService() : IAssemblyService {
    public Dictionary<string, string> GetActionList() => Assembly.GetExecutingAssembly().GetTypes()
        .Where(x => typeof(Controller).IsAssignableFrom(x)) // filter to controllers only
        .SelectMany(x => x.GetMethods())
        .Where(x => x.IsPublic // public methods only
            && !x.IsDefined(typeof(NonActionAttribute))  // action methods only
            && (x.IsDefined(typeof(AuthorizeAttribute)) || x.DeclaringType?.IsDefined(typeof(AuthorizeAttribute)) == true) // requires authorization or parent type does
            && !x.IsDefined(typeof(AllowAnonymousAttribute)) & !x.IsDefined(typeof(ParentActionAttribute))) // doesn't allow anonymous or methods with ParentAction
        .Select(x => $"{x.DeclaringType?.FullName?.Split('.').Last().StripController()}.{x.Name}")
        .Distinct()
        .ToDictionary(x => x.ToLower(CultureInfo.InvariantCulture), x => x);
}
