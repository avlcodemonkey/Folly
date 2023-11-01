using System.Globalization;
using Auth0.AspNetCore.Authentication;
using Folly.Attributes;
using Folly.Configuration;
using Folly.Extensions;
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

public class AccountController : BaseController {
    private readonly ILanguageService _LanguageService;
    private readonly IUserService _UserService;

    public AccountController(IAppConfiguration appConfig, IUserService userService, ILanguageService languageService, ILogger<AccountController> logger) : base(appConfig, logger) {
        _UserService = userService;
        _LanguageService = languageService;
    }

    public async Task Login(string returnUrl = "/") {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();
        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    public async Task Logout() {
        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action(nameof(DashboardController.Index), nameof(DashboardController).StripController()) }
        ).ConfigureAwait(false);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName), ParentAction(nameof(UpdateAccount))]
    public IActionResult ToggleContextHelp() {
        HttpContext.Session.ToggleSetting(Help.SettingName);
        return View("ToggleContextHelp", new Help(HttpContext.Session));
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName)]
    public async Task<IActionResult> UpdateAccount() {
        var user = await _UserService.GetUserByUserName(User.Identity!.Name!);
        return View("UpdateAccount", new UpdateAccount(user));
    }

    [HttpPost, Authorize(Policy = PermissionRequirementHandler.PolicyName), ValidModel]
    public async Task<IActionResult> UpdateAccount(UpdateAccount model) {
        if (!ModelState.IsValid)
            return View("UpdateAccount", model);

        var result = await _UserService.UpdateAccount(model);
        if (result.IsEmpty()) {
            var language = await _LanguageService.GetLanguageByIdAsync(model.LanguageId);
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo(language.LanguageCode))));
            ViewData[MessageProperty] = Account.AccountUpdated;
            return View("UpdateAccount", model);
        }
        ViewData[MessageProperty] = result;
        return View("UpdateAccount", model);
    }

    public IActionResult AccessDenied() {
        ViewData[MessageProperty] = Core.ErrorGeneric;
        return View("Error");
    }
}
