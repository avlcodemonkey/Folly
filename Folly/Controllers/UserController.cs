using Folly.Configuration;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class UserController : BaseController
{
    private readonly ILanguageService LanguageService;
    private readonly IUserService UserService;

    private IActionResult CreateEditView(User model) => View("CreateEdit", model);

    private async Task<User?> LoadUser(int id)
    {
        var model = await UserService.GetUserById(id);
        if (model != null)
            return model;

        ViewData[ErrorProperty] = Core.ErrorInvalidId;
        return null;
    }

    private async Task<IActionResult> Save(User model)
    {
        if (!ModelState.IsValid)
            return CreateEditView(model);

        await UserService.SaveUser(model);
        ViewData[MessageProperty] = Users.SuccessSavingUser;
        Response.Headers.Add("hx-push-url", Url.Action(nameof(Index)));
        return Index();
    }

    public UserController(IAppConfiguration appConfig, IUserService userService, ILanguageService languageService, ILogger<UserController> logger)
        : base(appConfig, logger)
    {
        UserService = userService;
        LanguageService = languageService;
    }

    [HttpGet]
    public IActionResult Create() => CreateEditView(new User());

    [HttpPost, ValidateAntiForgeryToken, ValidModel]
    public async Task<IActionResult> Create(User model) => await Save(model);

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await LoadUser(id);
        if (model == null)
            return Index();

        await UserService.DeleteUser(model);
        ViewData[MessageProperty] = Users.SuccessDeletingUser;
        return Index();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await LoadUser(id);
        return model == null ? Index() : CreateEditView(model);
    }

    [HttpPut, ValidateAntiForgeryToken, ValidModel]
    public async Task<IActionResult> Edit(User model) => await Save(model);

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List() => Ok((await UserService.GetAllUsers()).Select(x => new { x.Id, x.UserName, x.FirstName, x.LastName, x.Email }));
}
