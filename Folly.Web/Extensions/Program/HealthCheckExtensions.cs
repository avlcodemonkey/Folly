using Folly.Domain.Models;

namespace Folly.Extensions.Program;

public static class HealthCheckExtensions {

    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services) {
        services.AddHealthChecks().AddDbContextCheck<FollyDbContext>();

        return services;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app) {
        ((WebApplication)app).MapHealthChecks("/status");

        return app;
    }
}
