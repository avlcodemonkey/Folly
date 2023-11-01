using System.Globalization;
using Folly.Extensions;
using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Services;

public sealed class ViewService : IViewService {
    private readonly ILanguageService _LanguageService;
    private readonly IPermissionService _PermissionService;
    private readonly IRoleService _RoleService;

    public ViewService(ILanguageService languageService, IPermissionService permissionService, IRoleService roleService) {
        _LanguageService = languageService;
        _PermissionService = permissionService;
        _RoleService = roleService;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync() => await _RoleService.GetAllRolesAsync();

    public async Task<Dictionary<string, List<Permission>>> GetControllerPermissionsAsync() {
        var controllerPermissions = new Dictionary<string, List<Permission>>();
        var permissions = await _PermissionService.GetAll();

        foreach (var permission in permissions) {
            if (!controllerPermissions.ContainsKey(permission.ControllerName)) {
                controllerPermissions.Add(permission.ControllerName, new List<Permission>());
            }

            controllerPermissions[permission.ControllerName].Add(permission);
        }

        return controllerPermissions;
    }

    public async Task<IEnumerable<SelectListItem>> GetLanguageSelectListAsync()
        => (await _LanguageService.GetAllLanguagesAsync()).ToSelectList(x => x.Name, x => x.Id.ToString(CultureInfo.InvariantCulture));
}
