using Folly.Models;

namespace Folly.Services;

public interface IRoleService {
    Task<bool> CopyRole(CopyRole copyRoleDTO);

    Task<bool> DeleteRole(Role roleDTO);

    Task<IEnumerable<Role>> GetAllRoles();

    Task<Role> GetDefaultRole();

    Task<Role> GetRoleById(int id);

    Task<bool> SaveManyRolePermissions(IEnumerable<RolePermission> rolePermissions);

    Task<bool> SaveRole(Role roleDTO);
}
