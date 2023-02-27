﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Folly.Models;

namespace Folly.Services;

public class ViewService : IViewService
{
    private readonly ILanguageService LanguageService;
    private readonly IPermissionService PermissionService;
    private readonly IRoleService RoleService;

    public ViewService(ILanguageService languageService, IPermissionService permissionService, IRoleService roleService)
    {
        LanguageService = languageService;
        PermissionService = permissionService;
        RoleService = roleService;
    }

    public async Task<IEnumerable<Role>> GetAllRoles() => await RoleService.GetAllRoles();

    public async Task<Dictionary<string, List<Permission>>> GetControllerPermissions()
    {
        var controllerPermissions = new Dictionary<string, List<Permission>>();
        (await PermissionService.GetAll()).Each(permission => {
            if (!controllerPermissions.ContainsKey(permission.ControllerName))
                controllerPermissions.Add(permission.ControllerName, new List<Permission>());
            controllerPermissions[permission.ControllerName].Add(permission);
        });
        return controllerPermissions;
    }

    public async Task<IEnumerable<SelectListItem>> GetLanguageList() => (await LanguageService.GetAll()).ToSelectList(x => x.Name, x => x.Id.ToString());
}
