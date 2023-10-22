using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class RoleService : IRoleService {
    private readonly FollyDbContext _DbContext;

    public RoleService(FollyDbContext dbContext) => _DbContext = dbContext;

    public async Task<bool> CopyRole(DTO.CopyRole copyRoleDTO) {
        var role = await GetRoleById(copyRoleDTO.Id);
        if (role == null)
            return false;

        _DbContext.Roles.Add(new Role {
            Name = copyRoleDTO.Prompt, IsDefault = false,
            RolePermissions = (role.PermissionIds?.Select(x => new RolePermission { PermissionId = x }) ?? new List<RolePermission>()).ToList()
        });
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteRole(DTO.Role roleDTO) {
        var role = roleDTO.ToModel();
        _DbContext.Roles.Remove(role);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Role>> GetAllRoles() => await _DbContext.Roles.SelectDTO().ToListAsync();

    public async Task<DTO.Role> GetDefaultRole() => await _DbContext.Roles.SelectDTO().FirstAsync(x => x.IsDefault);

    public async Task<DTO.Role> GetRoleById(int id) => await _DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == id).SelectDTO().FirstAsync();

    public async Task<bool> SaveManyRolePermissions(IEnumerable<DTO.RolePermission> rolePermissions) {
        // @todo still need to re-test this
        await _DbContext.RolePermissions.AddRangeAsync(rolePermissions.Where(x => x.Id == 0).ToModel());
        _DbContext.RolePermissions.UpdateRange(rolePermissions.Where(x => x.Id > 0).ToModel());
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> SaveRole(DTO.Role roleDTO) {
        var role = roleDTO.ToModel();
        if (role.Id > 0) {
            var existingPermissions = await _DbContext.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync();
            role.RolePermissions.ForEach(x => {
                var existing = existingPermissions.FirstOrDefault(y => y.PermissionId == x.PermissionId);
                if (existing != null)
                    x.Id = existing.Id;
            });
            _DbContext.RolePermissions.RemoveRange(existingPermissions.Where(x => !role.RolePermissions.Any(y => y.PermissionId == x.PermissionId)));
            _DbContext.Roles.Update(role);
        } else {
            _DbContext.Roles.Add(role);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }
}
