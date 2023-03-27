using Folly.Configuration;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Controllers;

[Authorize(Policy = PermissionRequirementHandler.PolicyName)]
public class RoleController : BaseController
{
    private readonly IPermissionService PermissionService;
    private readonly IRoleService RoleService;

    private IActionResult CreateEditView(Role model) => View("CreateEdit", model);

    private async Task<Role?> LoadRole(int id)
    {
        var model = await RoleService.GetRoleById(id);
        if (model != null)
            return model;

        ViewData[ErrorProperty] = Core.ErrorInvalidId;
        return null;
    }

    private async Task<IActionResult> Save(Role model)
    {
        if (!ModelState.IsValid)
            return CreateEditView(model);

        await RoleService.SaveRole(model);
        ViewData[MessageProperty] = Roles.SuccessSavingRole;
        Response.Headers.Add(HtmxHeaders.PushUrl, Url.Action(nameof(Index)));
        return Index();
    }

    public RoleController(IAppConfiguration appConfig, IRoleService roleService, IPermissionService permissionService, ILogger<RoleController> logger) : base(appConfig, logger)
    {
        RoleService = roleService;
        PermissionService = permissionService;
    }

    [HttpGet, ParentAction(nameof(Edit))]
    public async Task<IActionResult> Copy(int id)
    {
        var model = await LoadRole(id);
        if (model == null)
            return Index();

        return View("Copy", new CopyRole { Id = model.Id, Prompt = Core.CopyOf.Replace("{0}", model.Name) });
    }

    [HttpPost, ParentAction(nameof(Edit)), ValidModel]
    public async Task<IActionResult> Copy(CopyRole model)
    {
        if (!ModelState.IsValid)
            return Index();

        await RoleService.CopyRole(model);
        ViewData[MessageProperty] = Roles.SuccessCopyingRole;
        return Index();
    }

    [HttpGet, ParentAction(nameof(Edit))]
    public IActionResult Create() => CreateEditView(new Role());

    [HttpPost, ParentAction(nameof(Edit)), ValidateAntiForgeryToken, ValidModel]
    public async Task<IActionResult> Create(Role model) => await Save(model);

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await LoadRole(id);
        if (model == null)
            return Index();

        await RoleService.DeleteRole(model);
        ViewData[MessageProperty] = Roles.SuccessDeletingRole;
        return Index();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await LoadRole(id);
        return model == null ? Index() : CreateEditView(model);
    }

    [HttpPut, ValidateAntiForgeryToken, ValidModel]
    public async Task<IActionResult> Edit(Role model) => await Save(model);

    [HttpGet]
    public IActionResult Index() => View("Index");

    [HttpGet, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List() => Ok((await RoleService.GetAllRoles()).Select(x => new { x.Id, x.Name }));

    [HttpGet]
    public async Task<IActionResult> RefreshPermissions()
    {
        await new PermissionManager(PermissionService, RoleService).Register();
        Response.Headers.Add(HtmxHeaders.ReplaceUrl, Url.Action(nameof(Index)));
        ViewData[MessageProperty] = Roles.SuccessRefreshingPermissions;
        return Index();
    }
}
