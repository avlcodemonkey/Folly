using System.Data;
using Folly.Constants;
using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class RoleService(FollyDbContext dbContext) : IRoleService {
    private readonly FollyDbContext _DbContext = dbContext;

    public async Task<bool> CopyRoleAsync(DTO.CopyRole copyRoleDTO) {
        var role = await _DbContext.Roles.Include(x => x.RolePermissions).FirstOrDefaultAsync(x => x.Id == copyRoleDTO.Id);
        if (role == null) {
            return false;
        }

        _DbContext.Roles.Add(new Role {
            Name = copyRoleDTO.Prompt, IsDefault = false,
            RolePermissions = role.RolePermissions.Select(x => new RolePermission { PermissionId = x.PermissionId }).ToList()
        });

        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteRoleAsync(int id) {
        // load role with rolePermissions so auditLog tracks them being deleted
        var role = await _DbContext.Roles.Include(x => x.RolePermissions).FirstOrDefaultAsync(x => x.Id == id);
        if (role == null) {
            return false;
        }

        _DbContext.Roles.Remove(role);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Role>> GetAllRolesAsync() => await _DbContext.Roles.SelectAsDTO().ToListAsync();

    public async Task<DTO.Role?> GetDefaultRoleAsync() => await _DbContext.Roles.Where(x => x.IsDefault).SelectAsDTO().FirstOrDefaultAsync();

    public async Task<DTO.Role?> GetRoleByIdAsync(int id)
        => await _DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == id).SelectAsDTO().FirstOrDefaultAsync();

    public async Task<ServiceResult> SaveRoleAsync(DTO.Role roleDTO) {
        if (roleDTO.Id > 0) {
            var role = await _DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == roleDTO.Id).FirstOrDefaultAsync();
            if (role == null) {
                return ServiceResult.InvalidId;
            }

            await MapToEntity(roleDTO, role);

            // set the original rowVersion value so concurrency check works correctly
            // https://stackoverflow.com/questions/44609991/ef-not-throwing-dbupdateconcurrencyexception-despite-conflicting-updates
            _DbContext.Entry(role).OriginalValues[nameof(role.RowVersion)] = roleDTO.RowVersion;

            _DbContext.Roles.Update(role);
        } else {
            var role = new Role();
            await MapToEntity(roleDTO, role);
            _DbContext.Roles.Add(role);
        }

        try {
            return await _DbContext.SaveChangesAsync() > 0 ? ServiceResult.Success : ServiceResult.GenericError;
        } catch (DbUpdateConcurrencyException ex) {
            return ServiceResult.ConcurrencyError;
        }
    }

    public async Task<bool> AddPermissionsToDefaultRoleAsync(IEnumerable<int> permissionIds) {
        // @todo still need to re-test this
        var defaultRole = await _DbContext.Roles.FirstOrDefaultAsync(x => x.IsDefault);
        if (defaultRole == null) {
            return false;
        }

        defaultRole.RolePermissions = permissionIds.Select(x => new RolePermission { PermissionId = x, RoleId = defaultRole.Id }).ToList();
        return await _DbContext.SaveChangesAsync() > 0;
    }

    private async Task MapToEntity(DTO.Role roleDTO, Role role) {
        role.Name = roleDTO.Name;
        role.IsDefault = roleDTO.IsDefault;

        var existingPermissions = new Dictionary<int, RolePermission>();
        if (role.Id > 0 && roleDTO.PermissionIds?.Any() == true) {
            existingPermissions = (await _DbContext.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync()).ToDictionary(x => x.PermissionId, x => x);
        }

        role.RolePermissions = roleDTO.PermissionIds?.Select(x => {
            if (existingPermissions.TryGetValue(x, out var rolePermission)) {
                return rolePermission;
            }
            return new RolePermission { PermissionId = x, RoleId = roleDTO.Id };
        }).ToList() ?? [];
    }
}
