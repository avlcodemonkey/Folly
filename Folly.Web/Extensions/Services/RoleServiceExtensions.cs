using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class RoleServiceExtensions {
    public static IQueryable<DTO.Role> SelectAsDTO(this IQueryable<Role> query)
        => query.Select(x => new DTO.Role { Id = x.Id, Name = x.Name, IsDefault = x.IsDefault, PermissionIds = x.RolePermissions.Select(x => x.PermissionId) });
}
