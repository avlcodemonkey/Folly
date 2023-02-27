using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Folly.Configuration;
using Folly.Services;

namespace Folly.Utils;

public static class Authentication
{
    public const string AuthCookieName = ".Folly.Auth";
    public const string SessionCookieName = ".Folly.Session";

    /// <summary>
    /// Configure authentication and session for app.
    /// </summary>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, AppConfiguration appConfig)
    {
        services.AddSession(x => {
            x.Cookie.HttpOnly = true;
            x.Cookie.Name = SessionCookieName;
            x.Cookie.IsEssential = true;
            x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            x.Cookie.SameSite = SameSiteMode.Lax;
        });

        services.AddAuthentication(x => {
            x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(x => {
            x.Cookie.HttpOnly = true;
            x.Cookie.Name = AuthCookieName;
            x.Cookie.IsEssential = true;
            x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            x.Cookie.SameSite = SameSiteMode.Lax;
        }).AddOpenIdConnect("Auth0", options => {
            // Set the authority to your Auth0 domain
            options.Authority = $"https://{appConfig.Auth.Domain}";

            // Configure the Auth0 Client ID and Client Secret
            options.ClientId = appConfig.Auth.ClientId;
            options.ClientSecret = appConfig.Auth.ClientSecret;

            // Set response type to code
            options.ResponseType = OpenIdConnectResponseType.Code;

            // Configure the scope
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            // use Auth0's unique identifier as Identity.Name
            options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;

            options.CallbackPath = new PathString($"/{nameof(Controllers.AccountController).StripController()}/Callback");

            // Configure the Claims Issuer to be Auth0
            options.ClaimsIssuer = "Auth0";

            options.Events = new OpenIdConnectEvents {
                // handle the logout redirection
                OnRedirectToIdentityProviderForSignOut = (context) => {
                    var logoutUri = $"https://{appConfig.Auth.Domain}/v2/logout?client_id={appConfig.Auth.ClientId}";
                    var postLogoutUri = context.Properties.RedirectUri;
                    if (!postLogoutUri.IsEmpty())
                    {
                        if (postLogoutUri.StartsWith("/"))
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
                    var userService = (UserService)serviceProvider.GetService(typeof(IUserService));
                    var languageService = (LanguageService)serviceProvider.GetService(typeof(ILanguageService));

                    var username = context.Principal.Identity.Name;
                    var user = await userService.GetUserByUsername(username);
                    var languages = await languageService.GetAll();
                    if (user == null)
                    {
                        user = new Models.User {
                            UserName = username,
                            FirstName = context.Principal.FindFirst(ClaimTypes.Name)?.Value ?? context.Principal.FindFirst(ClaimTypes.Name)?.Value,
                            Email = context.Principal.FindFirst(ClaimTypes.Email)?.Value,
                            LanguageId = languages.FirstOrDefault(x => x.IsDefault)?.Id ?? 0,
                            Status = true
                        };
                        await userService.AddUser(user);
                    }

                    var languageCode = languages.FirstOrDefault(x => x.Id == user.LanguageId).LanguageCode;
                    context.HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo(languageCode ?? "en"))));
                }
            };
        });

        return services;
    }
}
