using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class PermissionService(FollyDbContext dbContext) : IPermissionService {
    private readonly FollyDbContext _DbContext = dbContext;

    public async Task<bool> DeletePermissionAsync(int permissionId) {
        var permission = await _DbContext.Permissions.FirstOrDefaultAsync(x => x.Id == permissionId);
        if (permission == null) {
            return false;
        }

        _DbContext.Remove(permission);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Permission>> GetAllPermissionsAsync() => await _DbContext.Permissions.SelectAsDTO().ToListAsync();

    public async Task<bool> SavePermissionAsync(DTO.Permission permissionDTO) {
        if (permissionDTO.Id > 0) {
            var permission = await _DbContext.Permissions.Where(x => x.Id == permissionDTO.Id).FirstOrDefaultAsync();
            if (permission == null) {
                return false;
            }

            MapToEntity(permissionDTO, permission);
            _DbContext.Permissions.Update(permission);
        } else {
            var permission = new Permission();
            MapToEntity(permissionDTO, permission);
            _DbContext.Permissions.Add(permission);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }

    private static void MapToEntity(DTO.Permission permissionDTO, Permission permission) {
        permission.Id = permissionDTO.Id;
        permission.ControllerName = permissionDTO.ControllerName;
        permission.ActionName = permissionDTO.ActionName;
    }

    public async Task<Dictionary<string, List<DTO.Permission>>> GetControllerPermissionsAsync() {
        var controllerPermissions = new Dictionary<string, List<DTO.Permission>>();
        var permissions = await GetAllPermissionsAsync();

        foreach (var permission in permissions) {
            if (!controllerPermissions.TryGetValue(permission.ControllerName, out _)) {
                controllerPermissions.Add(permission.ControllerName, []);
            }
            controllerPermissions[permission.ControllerName].Add(permission);
        }

        return controllerPermissions;
    }
}
