using System.Globalization;
using System.Reflection;
using Folly.Attributes;
using Folly.Extensions;
using Folly.Models;
using Folly.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Folly.Utils;

public sealed class PermissionManager {
    private readonly IPermissionService _PermissionService;
    private readonly IRoleService _RoleService;

    public PermissionManager(IPermissionService permissionService, IRoleService roleService) {
        _PermissionService = permissionService;
        _RoleService = roleService;
    }

    /// <summary>
    /// Scans the assembly for all controllers and updates the permissions table to match the list of available actions.
    /// </summary>
    public async Task<bool> Register() {
        // build a list of all available actions
        var actionList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(Controller).IsAssignableFrom(x)) //filter controllers
            .SelectMany(x => x.GetMethods())
            .Where(x => x.IsPublic && !x.IsDefined(typeof(NonActionAttribute)) && (x.IsDefined(typeof(AuthorizeAttribute)) || x.DeclaringType!.IsDefined(typeof(AuthorizeAttribute)))
                && !x.IsDefined(typeof(AllowAnonymousAttribute)) & !x.IsDefined(typeof(ParentActionAttribute)))
            .Select(x => $"{x.DeclaringType?.FullName?.Split('.').Last().Replace("Controller", "")}.{x.Name}")
            .Distinct()
            .ToDictionary(x => x.ToLower(CultureInfo.InvariantCulture), x => x);

        // query all permissions from db
        var permissions = (await _PermissionService.GetAll()).ToDictionary(x => $"{x.ControllerName?.Trim()}.{x.ActionName?.Trim()}".ToLower(CultureInfo.InvariantCulture), x => x);

        // save any actions not in db
        await actionList.Where(x => !permissions.ContainsKey(x.Key)).ToList().ForEachAsync(async x => {
            var parts = x.Value.Split('.');
            await _PermissionService.Save(new Permission { ControllerName = parts[0], ActionName = parts[1] });
        });

        // delete any permission not in action list
        await permissions.Where(x => !actionList.ContainsKey(x.Key)).ToList().ForEachAsync(async x => await _PermissionService.Delete(x.Value.Id));

        // if there are no permissions in the db, then set up the default role with all permissions now that we've added permissions
        if (!permissions.Any()) {
            // reload permissions from the db and add all to the default role
            var defaultRole = (await _RoleService.GetAllRoles()).FirstOrDefault(x => x.IsDefault);
            if (defaultRole != null)
                await _RoleService.SaveManyRolePermissions((await _PermissionService.GetAll()).Select(x => new RolePermission { PermissionId = x.Id, RoleId = defaultRole.Id }));
        }
        return true;
    }
}
