using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Folly.Utils;

public class ControllerAction
{
    private static readonly List<Type> CheckableType = new() { typeof(HttpPostAttribute), typeof(HttpPutAttribute), typeof(HttpDeleteAttribute) };
    private static readonly string Namespace = typeof(Controllers.BaseController).Namespace;
    private readonly IMemoryCache Cache;

    private Type GetControllerType() => Assembly.GetExecutingAssembly().GetType($"{Namespace}.{Controller}Controller", false, true);

    private MethodInfo GetMethod()
    {
        var controllerType = GetControllerType();
        if (controllerType == null)
            return null;

        var requestType = RequestType();
        var methods = controllerType.GetMethods().Where(x => x.Name.ToLower() == Action.ToLower());
        if (requestType != null)
            return methods.FirstOrDefault(x => x.GetCustomAttributes(false).Any(a => a.GetType() == requestType));
        return methods.FirstOrDefault(x => !x.GetCustomAttributes(false).Any(a => CheckableType.Contains(a.GetType())));
    }

    private Type RequestType()
    {
        if (Method == HttpVerb.Post)
            return typeof(HttpPostAttribute);
        if (Method == HttpVerb.Put)
            return typeof(HttpPutAttribute);
        if (Method == HttpVerb.Delete)
            return typeof(HttpDeleteAttribute);
        return null;
    }

    public ControllerAction()
    { }

    public ControllerAction(IMemoryCache cache) => Cache = cache;

    public ControllerAction(string controller, string action, HttpVerb method = HttpVerb.Get)
    {
        Controller = controller;
        Action = action;
        Method = method;
    }

    public string Action { get; set; }
    public string Controller { get; set; }
    public HttpVerb Method { get; set; }

    public List<string> EffectivePermissions() => Cache.Cached($"effectivePermissions_{Controller}_{Action}_{Method}", () => {
        var method = GetMethod();
        if (method != null)
        {
            var parentAttr = method.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(ParentActionAttribute));
            if (parentAttr != null)
                return ((ParentActionAttribute)parentAttr).Action.ToLower().Trim().Split(',').Select(x => $"{Controller.ToLower().Trim()}.{x.Trim()}").ToList();
        }
        return new() { $"{Controller.ToLower().Trim()}.{Action.ToLower().Trim()}" };
    });
}
