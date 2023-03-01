using System.Globalization;
using Auth0.AspNetCore.Authentication;
using Folly.Configuration;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Petch]
public class AccountController : BaseController
{
    private const string AuthProfile = "profile";
    private const string AuthScheme = "Auth0";
    private const string AuthScope = "scope";
    private readonly ILanguageService LanguageService;
    private readonly IUserService UserService;

    public AccountController(IAppConfiguration appConfig, IUserService userService, ILanguageService languageService, ILogger<AccountController> logger) : base(appConfig, logger)
    {
        UserService = userService;
        LanguageService = languageService;
    }

    public async Task Login(string returnUrl = "/")
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(AuthScheme, new AuthenticationProperties {
            RedirectUri = Url.Action(nameof(DashboardController.Index), nameof(DashboardController).StripController())
        }).ConfigureAwait(false);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName), ParentAction(nameof(UpdateAccount))]
    public IActionResult ToggleContextHelp()
    {
        HttpContext.Session.ToggleSetting(Help.SettingName);
        return View("ToggleContextHelp", new Help(HttpContext.Session));
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName), ParentAction(nameof(UpdateAccount))]
    public IActionResult ToggleProfiling()
    {
        HttpContext.Session.ToggleSetting(Profiling.SettingName);
        return View("ToggleProfiling", new Profiling(HttpContext.Session));
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName)]
    public async Task<IActionResult> UpdateAccount()
    {
        var user = await UserService.GetUserByUsername(User.Identity.Name);
        return View("UpdateAccount", new UpdateAccount(user));
    }

    [HttpPost, Authorize(Policy = PermissionRequirementHandler.PolicyName), ValidModel]
    public async Task<IActionResult> UpdateAccount(UpdateAccount model)
    {
        if (!ModelState.IsValid)
            return View("UpdateAccount", model);

        var result = await UserService.UpdateAccount(model);
        if (result.IsEmpty())
        {
            var language = await LanguageService.GetLanguageById(model.LanguageId);
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo(language.LanguageCode))));
            ViewData[MessageProperty] = Account.AccountUpdated;
            return View("UpdateAccount", model);
        }
        ViewData[MessageProperty] = result;
        return View("UpdateAccount", model);
    }
}
