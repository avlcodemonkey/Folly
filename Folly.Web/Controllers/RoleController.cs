using Folly.Attributes;
using Folly.Constants;
using Folly.Extensions;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class RoleController(IRoleService roleService, IPermissionService permissionService, IAssemblyService assemblyService, ILogger<RoleController> logger)
    : BaseController(logger) {

    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;
    private readonly IAssemblyService _AssemblyService = assemblyService;

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List()
        => Ok((await _RoleService.GetAllRolesAsync()).Select(x => new RoleListResult { Id = x.Id, Name = x.Name }));

    private async Task<IActionResult> Save(Role model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return View("CreateEdit", model);
        }

        var result = await _RoleService.SaveRoleAsync(model);
        if (result != ServiceResult.Success) {
            ViewData.AddServiceError(result);
            return View("CreateEdit", model);
        }

        ViewData.AddMessage(Roles.SuccessSavingRole);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }

    [HttpGet, ParentAction(nameof(Edit))]
    public IActionResult Create() => View("CreateEdit", new Role());

    [HttpPost, ParentAction(nameof(Edit)), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Role model) => await Save(model);

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var model = await _RoleService.GetRoleByIdAsync(id);
        if (model == null) {
            ViewData.AddError(Core.ErrorInvalidId);
            return Index();
        }
        return View("CreateEdit", model);
    }

    [HttpPut, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Role model) => await Save(model);

    [HttpGet, ParentAction(nameof(Edit))]
    public async Task<IActionResult> Copy(int id) {
        var model = await _RoleService.GetRoleByIdAsync(id);
        if (model == null) {
            ViewData.AddError(Core.ErrorInvalidId);
            return Index();
        }

        return View("Copy", new CopyRole { Id = model.Id, Prompt = Core.CopyOf.Replace("{0}", model.Name) });
    }

    [HttpPost, ParentAction(nameof(Edit))]
    public async Task<IActionResult> Copy(CopyRole model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return View("Copy", model);
        }

        if (!await _RoleService.CopyRoleAsync(model)) {
            ViewData.AddError(Roles.ErrorSavingRole);
            return View("Copy", model);
        }

        ViewData.AddMessage(Roles.SuccessCopyingRole);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id) {
        if (!await _RoleService.DeleteRoleAsync(id)) {
            ViewData.AddError(Roles.ErrorDeletingRole);
            return Index();
        }

        ViewData.AddMessage(Roles.SuccessDeletingRole);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }

    [HttpGet]
    public async Task<IActionResult> RefreshPermissions() {
        if (!await new PermissionManager(_AssemblyService, _PermissionService, _RoleService).RegisterAsync()) {
            ViewData.AddError(Roles.ErrorRefreshingPermissions);
            return Index();
        }

        ViewData.AddMessage(Roles.SuccessRefreshingPermissions);
        PushUrl(Url.Action(nameof(Index)));
        return Index();
    }
}
