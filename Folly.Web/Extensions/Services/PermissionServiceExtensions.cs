using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class PermissionServiceExtensions {
    public static IQueryable<DTO.Permission> SelectAsDTO(this IQueryable<Permission> query)
        => query.Select(x => new DTO.Permission(x.Id, x.ControllerName, x.ActionName));
}
