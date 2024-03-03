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
public class UserController(IUserService userService, ILogger<UserController> logger) : BaseController(logger) {
    private readonly IUserService _UserService = userService;

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List()
        => Ok((await _UserService.GetAllUsersAsync()).Select(x =>
            new UserListResult { Id = x.Id, UserName = x.UserName, FirstName = x.FirstName, LastName = x.LastName ?? "", Email = x.Email }
        ));

    private async Task<IActionResult> Save(User model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return View("CreateEdit", model);
        }

        if (!await _UserService.SaveUserAsync(model)) {
            ViewData.AddError(Users.ErrorSavingUser);
            return View("CreateEdit", model);
        }

        ViewData.AddMessage(Users.SuccessSavingUser);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }

    [HttpGet]
    public IActionResult Create() => View("CreateEdit", new User());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User model) => await Save(model);

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var model = await _UserService.GetUserByIdAsync(id);
        if (model == null) {
            ViewData.AddError(Core.ErrorInvalidId);
            return Index();
        }
        return View("CreateEdit", model);
    }

    [HttpPut, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User model) => await Save(model);

    [HttpDelete]
    public async Task<IActionResult> Delete(int id) {
        if (!await _UserService.DeleteUserAsync(id)) {
            ViewData.AddError(Users.ErrorDeletingUser);
            return Index();
        }

        PushUrl(Url.Action(nameof(Index)));
        ViewData.AddMessage(Users.SuccessDeletingUser);
        return Index();
    }
}
