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
public class RoleController(IRoleService roleService, IPermissionService permissionService, IAssemblyService assemblyService, ILogger<RoleController> logger) : BaseController(logger) {
    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;
    private readonly IAssemblyService _AssemblyService = assemblyService;

    private ViewResult CreateEditView(Role model) => View("CreateEdit", model);

    private async Task<Role?> LoadRole(int id) {
        var model = await _RoleService.GetRoleByIdAsync(id);
        if (model == null) {
            ViewData.AddError(Core.ErrorInvalidId);
        }
        return model;
    }

    private async Task<IActionResult> Save(Role model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return CreateEditView(model);
        }

        await _RoleService.SaveRoleAsync(model);
        ViewData.AddMessage(Roles.SuccessSavingRole);
        PushAction(nameof(Index));
        return Index();
    }

    [HttpGet, ParentAction(nameof(Edit))]
    public async Task<IActionResult> Copy(int id) {
        var model = await LoadRole(id);
        if (model == null) {
            return Index();
        }

        return View("Copy", new CopyRole { Id = model.Id, Prompt = Core.CopyOf.Replace("{0}", model.Name) });
    }

    [HttpPost, ParentAction(nameof(Edit))]
    public async Task<IActionResult> Copy(CopyRole model) {
        if (!ModelState.IsValid) {
            ViewData.AddError(ModelState);
            return Index();
        }

        await _RoleService.CopyRoleAsync(model);
        PushAction(nameof(Index));
        ViewData.AddMessage(Roles.SuccessCopyingRole);
        return Index();
    }

    [HttpGet, ParentAction(nameof(Edit))]
    public IActionResult Create() => CreateEditView(new Role());

    [HttpPost, ParentAction(nameof(Edit)), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Role model) => await Save(model);

    [HttpDelete]
    public async Task<IActionResult> Delete(int id) {
        await _RoleService.DeleteRoleAsync(id);
        ViewData.AddMessage(Roles.SuccessDeletingRole);
        return Index();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var model = await LoadRole(id);
        return model == null ? Index() : CreateEditView(model);
    }

    [HttpPut, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Role model) => await Save(model);

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List() => Ok((await _RoleService.GetAllRolesAsync()).Select(x => new { x.Id, x.Name }));

    [HttpGet]
    public async Task<IActionResult> RefreshPermissions() {
        await new PermissionManager(_AssemblyService, _PermissionService, _RoleService).RegisterAsync();
        ViewData.AddMessage(Roles.SuccessRefreshingPermissions);
        return Index();
    }
}
