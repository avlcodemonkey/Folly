using Folly.Domain.Models;
using Folly.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class RoleService : IRoleService {
    private readonly FollyDbContext DbContext;

    public RoleService(FollyDbContext dbContext) => DbContext = dbContext;

    public async Task<bool> CopyRole(DTO.CopyRole copyRoleDTO) {
        var role = await GetRoleById(copyRoleDTO.Id);
        if (role == null)
            return false;

        DbContext.Roles.Add(new Role {
            Name = copyRoleDTO.Prompt, IsDefault = false,
            RolePermissions = (role.PermissionIds?.Select(x => new RolePermission { PermissionId = x }) ?? new List<RolePermission>()).ToList()
        });
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteRole(DTO.Role roleDTO) {
        var role = roleDTO.ToModel();
        DbContext.Roles.Remove(role);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.Role>> GetAllRoles() => await DbContext.Roles.SelectDTO().ToListAsync();

    public async Task<DTO.Role> GetDefaultRole() => await DbContext.Roles.SelectDTO().FirstAsync(x => x.IsDefault);

    public async Task<DTO.Role> GetRoleById(int id) => await DbContext.Roles.Include(x => x.RolePermissions).Where(x => x.Id == id).SelectDTO().FirstAsync();

    public async Task<bool> SaveManyRolePermissions(IEnumerable<DTO.RolePermission> rolePermissions) {
        // @todo still need to re-test this
        await DbContext.RolePermissions.AddRangeAsync(rolePermissions.Where(x => x.Id == 0).ToModel());
        DbContext.RolePermissions.UpdateRange(rolePermissions.Where(x => x.Id > 0).ToModel());
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> SaveRole(DTO.Role roleDTO) {
        var role = roleDTO.ToModel();
        if (role.Id > 0) {
            var existingPermissions = await DbContext.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync();
            role.RolePermissions.ForEach(x => {
                var existing = existingPermissions.FirstOrDefault(y => y.PermissionId == x.PermissionId);
                if (existing != null)
                    x.Id = existing.Id;
            });
            DbContext.RolePermissions.RemoveRange(existingPermissions.Where(x => !role.RolePermissions.Any(y => y.PermissionId == x.PermissionId)));
            DbContext.Roles.Update(role);
        }
        else {
            DbContext.Roles.Add(role);
        }

        return await DbContext.SaveChangesAsync() > 0;
    }
}
