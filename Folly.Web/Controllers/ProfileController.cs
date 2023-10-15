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

public class ProfileController : BaseController {
    private readonly ILanguageService LanguageService;
    private readonly IUserService UserService;

    public ProfileController(IAppConfiguration appConfig, IUserService userService, ILanguageService languageService, ILogger<ProfileController> logger) : base(appConfig, logger) {
        UserService = userService;
        LanguageService = languageService;
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

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName), ParentAction(nameof(UpdateProfile))]
    public IActionResult ToggleContextHelp() {
        HttpContext.Session.ToggleSetting(Help.SettingName);
        return View("ToggleContextHelp", new Help(HttpContext.Session));
    }

    [HttpGet, Authorize(Policy = PermissionRequirementHandler.PolicyName)]
    public async Task<IActionResult> UpdateProfile() {
        var user = await UserService.GetUserByUserName(User.Identity!.Name!);
        return View("UpdateProfile", new UpdateProfile(user));
    }

    [HttpPost, Authorize(Policy = PermissionRequirementHandler.PolicyName), ValidModel]
    public async Task<IActionResult> UpdateProfile(UpdateProfile model) {
        if (!ModelState.IsValid)
            return View("UpdateProfile", model);

        var result = await UserService.UpdateProfile(model);
        if (result.IsEmpty()) {
            var language = await LanguageService.GetLanguageById(model.LanguageId);
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo(language.LanguageCode))));
            ViewData[MessageProperty] = Profile.ProfileUpdated;
            return View("UpdateProfile", model);
        }
        ViewData[MessageProperty] = result;
        return View("UpdateProfile", model);
    }

    public IActionResult AccessDenied() {
        ViewData[MessageProperty] = Core.ErrorGeneric;
        return View("Error");
    }
}
