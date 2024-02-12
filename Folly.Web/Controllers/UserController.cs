using Folly.Attributes;
using Folly.Extensions;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class UserController(IUserService userService, ILanguageService languageService, ILogger<UserController> logger) : BaseController(logger) {
    private readonly ILanguageService _LanguageService = languageService;
    private readonly IUserService _UserService = userService;

    private ViewResult CreateEditView(User model) => View("CreateEdit", model);

    private async Task<User?> LoadUser(int id) {
        var model = await _UserService.GetUserByIdAsync(id);
        if (model != null) {
            return model;
        }

        ViewData.AddError(Core.ErrorInvalidId);
        return null;
    }

    private async Task<IActionResult> Save(User model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return CreateEditView(model);
        }

        await _UserService.SaveUserAsync(model);
        ViewData.AddMessage(Users.SuccessSavingUser);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }

    [HttpGet]
    public IActionResult Create() => CreateEditView(new User());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User model) => await Save(model);

    [HttpDelete]
    public async Task<IActionResult> Delete(int id) {
        await _UserService.DeleteUserAsync(id);
        ViewData.AddMessage(Users.SuccessDeletingUser);
        return Index();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var model = await LoadUser(id);
        return model == null ? Index() : CreateEditView(model);
    }

    [HttpPut, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User model) => await Save(model);

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List()
        => Ok((await _UserService.GetAllUsersAsync()).Select(x => new { x.Id, x.UserName, x.FirstName, x.LastName, x.Email }));
}
