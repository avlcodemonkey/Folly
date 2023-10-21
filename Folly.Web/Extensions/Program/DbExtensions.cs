using Folly.Domain.Models;
using Folly.Services;

namespace Folly.Extensions.Program;

public static class DbExtensions {

    public static IServiceCollection ConfigureDb(this IServiceCollection services) {
        services.AddDbContext<FollyDbContext>();

        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
