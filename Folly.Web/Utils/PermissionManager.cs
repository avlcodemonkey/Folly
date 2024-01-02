using System.Globalization;
using System.Reflection;
using Folly.Attributes;
using Folly.Extensions;
using Folly.Models;
using Folly.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Utils;

public sealed class PermissionManager(IPermissionService permissionService, IRoleService roleService) {
    private readonly IPermissionService _PermissionService = permissionService;
    private readonly IRoleService _RoleService = roleService;

    /// <summary>
    /// Scans the assembly for all controllers and updates the permissions table to match the list of available actions.
    /// </summary>
    public async Task<bool> Register() {
        // @todo need to re-test this method

        // build a list of all available actions
        var actionList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(Controller).IsAssignableFrom(x)) //filter controllers
            .SelectMany(x => x.GetMethods())
            .Where(x => x.IsPublic && !x.IsDefined(typeof(NonActionAttribute))
                && (x.IsDefined(typeof(AuthorizeAttribute)) || x.DeclaringType!.IsDefined(typeof(AuthorizeAttribute)))
                && !x.IsDefined(typeof(AllowAnonymousAttribute)) & !x.IsDefined(typeof(ParentActionAttribute)))
            .Select(x => $"{x.DeclaringType?.FullName?.Split('.').Last().StripController()}.{x.Name}")
            .Distinct()
            .ToDictionary(x => x.ToLower(CultureInfo.InvariantCulture), x => x);

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
