using Folly.Models;

namespace Folly.Services;

public interface IPermissionService {
    Task<bool> DeletePermissionAsync(int permissionId);

    Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    Task<bool> SavePermissionAsync(Permission permissionDTO);

    Task<Dictionary<string, List<Permission>>> GetControllerPermissionsAsync();
}
