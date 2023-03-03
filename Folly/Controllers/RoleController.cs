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

    private async Task<Role> LoadRole(int id, bool useTempData = false)
    {
        Role model;
        if ((model = await RoleService.GetRoleById(id)) != null)
            return model;

        if (useTempData)
            TempData[ErrorProperty] = Core.ErrorInvalidId;
        else
            ViewData[ErrorProperty] = Core.ErrorInvalidId;
        return null;
    }

    private async Task<IActionResult> Save(Role model)
    {
        if (!ModelState.IsValid)
            return CreateEditView(model);

        await RoleService.SaveRole(model);
        ViewData[MessageProperty] = Roles.SuccessSavingRole;
        return Index();
    }

    public RoleController(IAppConfiguration appConfig, IRoleService roleService, IPermissionService permissionService, ILogger<RoleController> logger) : base(appConfig, logger)
    {
        RoleService = roleService;
        PermissionService = permissionService;
    }

    [HttpGet, ParentAction(nameof(Edit)), ValidModel]
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

    [HttpDelete, AjaxRequestOnly]
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
    public IActionResult Index()
    {
        RouteData.Values.Remove(IDParameter);
        return View("Index");
    }

    [HttpPost, ParentAction(nameof(Index)), AjaxRequestOnly]
    public async Task<IActionResult> List() => Rows((await RoleService.GetAllRoles()).Select(x => new { x.Id, x.Name }));

    [HttpGet, AjaxRequestOnly]
    public async Task<IActionResult> RefreshPermissions()
    {
        await new Permissions(PermissionService, RoleService).Register();
        ViewData[MessageProperty] = Roles.SuccessRefreshingPermissions;
        return Index();
    }
}
