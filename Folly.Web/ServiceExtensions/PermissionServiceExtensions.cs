using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.ServiceExtensions;

public static class PermissionServiceExtensions {
    public static IQueryable<DTO.Permission> SelectDTO(this IQueryable<Permission> query)
        => query.Select(x => new DTO.Permission { Id = x.Id, ControllerName = x.ControllerName, ActionName = x.ActionName });

    public static Permission ToModel(this DTO.Permission permissionDTO)
        => new() { Id = permissionDTO.Id, ControllerName = permissionDTO.ControllerName, ActionName = permissionDTO.ActionName };
}
