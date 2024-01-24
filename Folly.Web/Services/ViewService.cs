using System.Globalization;
using Folly.Extensions;
using Folly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Folly.Services;

public sealed class ViewService(ILanguageService languageService, IPermissionService permissionService, IRoleService roleService, IUserService userService) : IViewService {
    private readonly ILanguageService _LanguageService = languageService;
    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;
    private readonly IUserService _UserService = userService;

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

    public async Task<IEnumerable<SelectListItem>> GetUserSelectListAsync()
        => (await _UserService.GetAllUsersAsync()).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToSelectList(
            x => string.Join(", ", new string[] { x.LastName ?? "", x.FirstName ?? "" }.Where(x => !string.IsNullOrWhiteSpace(x))),
            x => x.Id.ToString(CultureInfo.InvariantCulture)
        );

    public IEnumerable<SelectListItem> GetEntityStatesSelectList()
        => new List<EntityState> { EntityState.Deleted, EntityState.Added, EntityState.Modified }
            .Select(x => new SelectListItem(x.ToString(), x.ToString())).ToList();
}
