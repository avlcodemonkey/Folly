using Folly.Models;

namespace Folly.Services;

public interface IPermissionService {
    Task<bool> Delete(int permissionId);

    Task<IEnumerable<Permission>> GetAll();

    Task<bool> Save(Permission permissionDTO);
}
