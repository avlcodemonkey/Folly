using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Folly.Utils;

internal class SerilogMiddleware
{
    private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    private static readonly List<string> IgnoredPaths = new() { "profiler" };
    private readonly RequestDelegate Next;

    private static Serilog.ILogger LogWithContext(HttpContext httpContext)
    {
        var request = httpContext.Request;
        var result = Log
            .ForContext("RequestHeaders", request.Headers.Where(x => !x.Key.IsPrivate()).ToDictionary(h => h.Key, h => h.Value.ToString()), true)
            .ForContext("RequestQueryString", request.Query.Where(x => !x.Key.IsPrivate()).ToDictionary(h => h.Key, h => h.Value.ToString()), true)
            .ForContext("RequestHost", request.Host)
            .ForContext("RequestProtocol", request.Protocol);
        if (request.HasFormContentType)
            result = result.ForContext("RequestForm", request.Form.Where(x => !x.Key.IsPrivate()).ToDictionary(v => v.Key, v => v.Value.ToString()), true);
        return result;
    }

    public SerilogMiddleware(RequestDelegate next) => Next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var sw = Stopwatch.StartNew();
        await Next(httpContext).ConfigureAwait(true);
        sw.Stop();

        var path = httpContext.Request.Path.Value.ToLower();
        if (IgnoredPaths.Any(x => path.Contains(x)))
            return;

        var statusCode = httpContext.Response?.StatusCode;
        var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
        LogWithContext(httpContext).Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
    }
}
