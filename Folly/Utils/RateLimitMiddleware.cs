using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Folly.Resources;

namespace Folly.Utils;

public static class RateLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomIpRateLimiting(this IApplicationBuilder builder) => builder.UseMiddleware<IpRateLimitMiddlewareCustom>();
}

public class IpRateLimitMiddlewareCustom : IpRateLimitMiddleware
{
    private readonly ILogger Logger;

    protected override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter, RateLimitRule rule)
        => Logger.LogWarning(Core.RateLoggerWarning, identity.HttpVerb.ToUpper(), identity.Path, identity.ClientIp, rule.Limit, rule.Period, counter.Count, rule.Endpoint, httpContext.TraceIdentifier);

    public IpRateLimitMiddlewareCustom(RequestDelegate next, IProcessingStrategy strategy, IOptions<IpRateLimitOptions> options, IIpPolicyStore policyStore,
            IRateLimitConfiguration config, ILogger<IpRateLimitMiddleware> logger) : base(next, strategy, options, policyStore, config, logger)
        => Logger = logger;
}
