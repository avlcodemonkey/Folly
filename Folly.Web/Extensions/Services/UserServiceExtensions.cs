using Folly.Domain.Models;
using DTO = Folly.Models;

namespace Folly.Extensions.Services;

public static class UserServiceExtensions {
    public static IQueryable<DTO.User> SelectAsDTO(this IQueryable<User> query)
        => query.Select(x => new DTO.User {
            Id = x.Id, Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, UserName = x.UserName,
            LanguageId = x.LanguageId, RoleIds = x.UserRoles.Select(x => x.RoleId)
        });
}
