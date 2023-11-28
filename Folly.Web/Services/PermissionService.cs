using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class PermissionService : IPermissionService {
    private readonly FollyDbContext _DbContext;

    public PermissionService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<bool> DeletePermissionAsync(int permissionId) {
        var permission = await _DbContext.Permissions.FirstAsync(x => x.Id == permissionId);
        _DbContext.Remove(permission);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Permission>> GetAllPermissionsAsync() => await _DbContext.Permissions.SelectAsDTO().ToListAsync();

    public async Task<bool> SavePermissionAsync(DTO.Permission permissionDTO) {
        if (permissionDTO.Id > 0) {
            var permission = await _DbContext.Permissions.Where(x => x.Id == permissionDTO.Id).FirstAsync();
            MapUpdates(permissionDTO, permission);
            _DbContext.Permissions.Update(permission);
        } else {
            var permission = new Permission();
            MapUpdates(permissionDTO, permission);
            _DbContext.Permissions.Add(permission);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }

    private static void MapUpdates(DTO.Permission permissionDTO, Permission permission) {
        permission.Id = permissionDTO.Id;
        permission.ControllerName = permissionDTO.ControllerName;
        permission.ActionName = permissionDTO.ActionName;
    }
}
