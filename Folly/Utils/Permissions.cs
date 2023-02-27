﻿using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Folly.Models;
using Folly.Services;

namespace Folly.Utils;

public class Permissions
{
    private readonly IPermissionService PermissionService;
    private readonly IRoleService RoleService;

    public Permissions(IPermissionService permissionService, IRoleService roleService)
    {
        PermissionService = permissionService;
        RoleService = roleService;
    }

    /// <summary>
    /// Scans the assembly for all controllers and updates the permissions table to match the list of available actions.
    /// </summary>
    public async Task<bool> Register()
    {
        // build a list of all available actions
        var actionList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(Controller).IsAssignableFrom(x)) //filter controllers
            .SelectMany(x => x.GetMethods())
            .Where(x => x.IsPublic && !x.IsDefined(typeof(NonActionAttribute))
                 && (x.IsDefined(typeof(AuthorizeAttribute)) || (x.DeclaringType.IsDefined(typeof(AuthorizeAttribute)))) && !x.IsDefined(typeof(AllowAnonymousAttribute)) &!x.IsDefined(typeof(ParentActionAttribute)))
            .Select(x => $"{x.DeclaringType.FullName.Split('.').Last().Replace("Controller", "")}.{x.Name}")
            .Distinct()
            .ToDictionary(x => x.ToLower(), x => x);

        actionList.Add("profiler.dashboard", "Profiler.Dashboard");
        // query all permissions from db
        var permissions = (await PermissionService.GetAll()).ToDictionary(x => $"{x.ControllerName?.Trim()}.{x.ActionName?.Trim()}".ToLower(), x => x);

        // save any actions not in db
        actionList.Where(x => !permissions.ContainsKey(x.Key)).Each(async x => {
            var parts = x.Value.Split('.');
            await PermissionService.Save(new Permission { ControllerName = parts[0], ActionName = parts[1] });
        });
        // delete any permission not in action list
        permissions.Where(x => !actionList.ContainsKey(x.Key)).Each(async x => {
            await PermissionService.Delete(x.Value.Id);
        });

        // if there are no permissions in the db, then set up the default role with all permissions now that we've added permissions
        if (!permissions.Any())
        {
            // reload permissions from the db and add all to the default role
            var defaultRole = (await RoleService.GetAllRoles()).FirstOrDefault(x => x.IsDefault);
            if (defaultRole != null)
                await RoleService.SaveManyRolePermissions((await PermissionService.GetAll()).Select(x => new RolePermission { PermissionId = x.Id, RoleId = defaultRole.Id }));
        }
        return true;
    }
}
