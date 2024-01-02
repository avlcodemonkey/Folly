using System.Globalization;
using Folly.Extensions;
using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Services;

public sealed class ViewService(ILanguageService languageService, IPermissionService permissionService, IRoleService roleService) : IViewService {
    private readonly ILanguageService _LanguageService = languageService;
    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;

    public async Task<IEnumerable<Role>> GetAllRolesAsync() => await _RoleService.GetAllRolesAsync();

    public async Task<Dictionary<string, List<Permission>>> GetControllerPermissionsAsync() {
        var controllerPermissions = new Dictionary<string, List<Permission>>();
        var permissions = await _PermissionService.GetAllPermissionsAsync();

        foreach (var permission in permissions) {
            if (!controllerPermissions.TryGetValue(permission.ControllerName, out _)) {
                controllerPermissions.Add(permission.ControllerName, []);
            }
            controllerPermissions[permission.ControllerName].Add(permission);
        }

        return controllerPermissions;
    }

    public async Task<IEnumerable<SelectListItem>> GetLanguageSelectListAsync()
        => (await _LanguageService.GetAllLanguagesAsync()).ToSelectList(x => x.Name, x => x.Id.ToString(CultureInfo.InvariantCulture));
}
