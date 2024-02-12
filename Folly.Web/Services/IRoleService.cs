using Folly.Models;

namespace Folly.Services;

public interface IRoleService {
    Task<bool> CopyRoleAsync(CopyRole copyRoleDTO);

    Task<bool> DeleteRoleAsync(int id);

    Task<IEnumerable<Role>> GetAllRolesAsync();

    Task<Role?> GetDefaultRoleAsync();

    Task<Role?> GetRoleByIdAsync(int id);

    Task<bool> SaveRoleAsync(Role roleDTO);

    Task<bool> AddPermissionsToDefaultRoleAsync(IEnumerable<int> permissionIds);
}
