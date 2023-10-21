using System.Globalization;
using Folly.Extensions;
using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Services;

public sealed class ViewService : IViewService {
    private readonly ILanguageService LanguageService;
    private readonly IPermissionService PermissionService;
    private readonly IRoleService RoleService;

    public ViewService(ILanguageService languageService, IPermissionService permissionService, IRoleService roleService) {
        LanguageService = languageService;
        PermissionService = permissionService;
        RoleService = roleService;
    }

    public async Task<IEnumerable<Role>> GetAllRoles() => await RoleService.GetAllRoles();

    public async Task<Dictionary<string, List<Permission>>> GetControllerPermissions() {
        var controllerPermissions = new Dictionary<string, List<Permission>>();
        (await PermissionService.GetAll()).ToList().ForEach(permission => {
            if (!controllerPermissions.ContainsKey(permission.ControllerName))
                controllerPermissions.Add(permission.ControllerName, new List<Permission>());
            controllerPermissions[permission.ControllerName].Add(permission);
        });
        return controllerPermissions;
    }

    public async Task<IEnumerable<SelectListItem>> GetLanguageList() => (await LanguageService.GetAll()).ToSelectList(x => x.Name, x => x.Id.ToString(CultureInfo.InvariantCulture));
}
