using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.ServiceExtensions;

public static class RoleServiceExtensions {
    public static IQueryable<DTO.Role> SelectDTO(this IQueryable<Role> query)
        => query.Select(x => new DTO.Role { Id = x.Id, Name = x.Name, IsDefault = x.IsDefault, PermissionIds = x.RolePermissions.Select(x => x.PermissionId) });

    public static IEnumerable<RolePermission> ToModel(this IEnumerable<DTO.RolePermission> list)
        => list.Select(x => new RolePermission { Id = x.Id, PermissionId = x.PermissionId, RoleId = x.RoleId });

    public static Role ToModel(this DTO.Role roleDTO) {
        var model = new Role {
            Id = roleDTO.Id, Name = roleDTO.Name, IsDefault = roleDTO.IsDefault,
            RolePermissions = roleDTO.PermissionIds?.Select(x => new RolePermission { PermissionId = x, RoleId = roleDTO.Id }).ToList() ?? new List<RolePermission>()
        };
        return model;
    }
}
