using System.Globalization;
using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Folly.Configuration;
using Folly.Controllers;
using Folly.Extensions;
using Folly.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Localization;

namespace Folly.Utils;

public static class Authentication {
    /// <summary>
    /// Configure authentication and session for app using Auth0.
    /// </summary>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, AppConfiguration appConfig) {
        services.AddAuth0WebAppAuthentication(options => {
            options.Domain = appConfig.Auth.Domain;
            options.ClientId = appConfig.Auth.ClientId;
            options.CallbackPath = new PathString($"/{nameof(AccountController).StripController()}/Callback");
            options.Scope = "openid profile email";

            options.OpenIdConnectEvents = new OpenIdConnectEvents {
                // handle the logout redirection
                OnRedirectToIdentityProviderForSignOut = (context) => {
                    var logoutUri = $"https://{appConfig.Auth.Domain}/v2/logout?client_id={appConfig.Auth.ClientId}";
                    var postLogoutUri = context.Properties.RedirectUri;
                    if (!string.IsNullOrWhiteSpace(postLogoutUri)) {
                        if (postLogoutUri.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                            // transform to absolute
                            postLogoutUri = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase + postLogoutUri;
                        logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                    }

                    context.Response.Redirect(logoutUri);
                    context.HandleResponse();

                    return Task.CompletedTask;
                },
                OnTokenValidated = async (context) => {
                    var serviceProvider = services.BuildServiceProvider();
                    var username = context.Principal?.Identity?.Name;

                    if (context.Principal == null || serviceProvider.GetService(typeof(IUserService)) is not UserService userService ||
                        serviceProvider.GetService(typeof(ILanguageService)) is not LanguageService languageService || string.IsNullOrWhiteSpace(username)) {
                        return;
                    }

                    var user = await userService.GetUserByUserName(username);
                    var languages = await languageService.GetAll();
                    if (user == null) {
                        user = new Models.User {
                            UserName = username,
                            FirstName = context.Principal.FindFirst(ClaimTypes.Name)?.Value ?? context.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
                            Email = context.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "",
                            LanguageId = languages.FirstOrDefault(x => x.IsDefault)?.Id ?? 0,
                            Status = true
                        };
                        await userService.AddUser(user);
                    }

                    var languageCode = languages.FirstOrDefault(x => x.Id == user.LanguageId)?.LanguageCode ?? "en";
                    context.HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo(languageCode ?? "en"))));
                }
            };
        });

        // work around to move the Auth0 unique identifier into the Identity.Name
        services.Configure<OpenIdConnectOptions>(Auth0Constants.AuthenticationScheme, options => options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier);

        return services;
    }
}
