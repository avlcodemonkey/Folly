using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class RoleService : IRoleService {
    private readonly FollyDbContext _DbContext;

    private static void MapUpdates(DTO.Role roleDTO, Role role) {
        role.Name = roleDTO.Name;
        role.IsDefault = roleDTO.IsDefault;
        role.RolePermissions = roleDTO.PermissionIds?.Select(x => new RolePermission { PermissionId = x, RoleId = roleDTO.Id }).ToList() ?? new List<RolePermission>();
    }

    public RoleService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<bool> CopyRoleAsync(DTO.CopyRole copyRoleDTO) {
        var role = await _DbContext.Roles.Include(x => x.RolePermissions).FirstAsync(x => x.Id == copyRoleDTO.Id);

        _DbContext.Roles.Add(new Role {
            Name = copyRoleDTO.Prompt, IsDefault = false,
            RolePermissions = role.RolePermissions.Select(x => new RolePermission { PermissionId = x.PermissionId }).ToList()
        });

        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteRoleAsync(int id) {
        var role = await _DbContext.Roles.FirstAsync(x => x.Id == id);
        _DbContext.Roles.Remove(role);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Role>> GetAllRolesAsync() => await _DbContext.Roles.SelectAsDTO().ToListAsync();

    public async Task<DTO.Role> GetDefaultRoleAsync() => await _DbContext.Roles.SelectAsDTO().FirstAsync(x => x.IsDefault);

    public async Task<DTO.Role> GetRoleByIdAsync(int id)
        => await _DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == id).SelectAsDTO().FirstAsync();

    public async Task<bool> SaveRoleAsync(DTO.Role roleDTO) {
        if (roleDTO.Id > 0) {
            var role = await _DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == roleDTO.Id).FirstAsync();
            MapUpdates(roleDTO, role);
        } else {
            var role = new Role();
            MapUpdates(roleDTO, role);
            _DbContext.Roles.Add(role);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddPermissionsToDefaultRoleAsync(IEnumerable<int> permissionIds) {
        // @todo still need to re-test this

        var defaultRole = await _DbContext.Roles.FirstAsync(x => x.IsDefault);
        defaultRole.RolePermissions = permissionIds.Select(x => new RolePermission { PermissionId = x, RoleId = defaultRole.Id }).ToList();
        return await _DbContext.SaveChangesAsync() > 0;
    }
}
