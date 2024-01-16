using System.Globalization;
using Folly.Models;
using Folly.Services;

namespace Folly.Utils;

public sealed class PermissionManager(IAssemblyService assemblyService, IPermissionService permissionService, IRoleService roleService) {
    private readonly IAssemblyService _AssemblyService = assemblyService;
    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;

    /// <summary>
    /// Scans the assembly for all controllers and updates the permissions table to match the list of available actions.
    /// </summary>
    public async Task<bool> RegisterAsync() {
        // build a list of all available actions
        var actionList = _AssemblyService.GetActionList();

        // query all permissions from db
        var permissions = (await _PermissionService.GetAllPermissionsAsync())
            .ToDictionary(x => $"{x.ControllerName?.Trim()}.{x.ActionName?.Trim()}".ToLower(CultureInfo.InvariantCulture), x => x);

        // save any actions not in db
        var missingActionList = actionList.Where(x => !permissions.ContainsKey(x.Key));
        foreach (var permission in missingActionList) {
            var parts = permission.Value.Split('.');
            await _PermissionService.SavePermissionAsync(new Permission { ControllerName = parts[0], ActionName = parts[1] });
        }

        // delete any permission not in action list
        var removedPermissions = permissions.Where(x => !actionList.ContainsKey(x.Key));
        foreach (var permission in removedPermissions) {
            await _PermissionService.DeletePermissionAsync(permission.Value.Id);
        }

        // if there are no permissions in the db, then set the default role with all permissions now that we've added them
        if (permissions.Count == 0) {
            var permissionIds = (await _PermissionService.GetAllPermissionsAsync()).Select(x => x.Id);
            await _RoleService.AddPermissionsToDefaultRoleAsync(permissionIds);
        }

        return true;
    }
}
