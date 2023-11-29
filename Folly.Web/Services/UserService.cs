using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class UserService : IUserService {
    private readonly FollyDbContext _DbContext;
    private readonly IHttpContextAccessor _HttpContextAccessor;

    public UserService(FollyDbContext dbContext, IHttpContextAccessor httpContextAccessor) {
        _DbContext = dbContext;
        _HttpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> DeleteUserAsync(int id) {
        var user = await _DbContext.Users.FirstAsync(x => x.Id == id && x.Status == true);
        user.Status = false;
        _DbContext.Users.Update(user);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.User>> GetAllUsersAsync() => await _DbContext.Users.Where(x => x.Status == true).SelectAsDTO().ToListAsync();

    public async Task<IEnumerable<DTO.UserClaim>> GetClaimsByUserIdAsync(int id)
        => await _DbContext.UserRoles
            .Include(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                    .ThenInclude(x => x.Permission)
            .Where(x => x.UserId == id)
            .SelectMany(x => x.Role.RolePermissions.Select(y => y.Permission))
            .Select(x => new DTO.UserClaim { Id = x.Id, ActionName = x.ActionName, ControllerName = x.ControllerName })
            .ToListAsync();

    public async Task<DTO.User> GetUserByIdAsync(int id)
        => await _DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == id && x.Status == true).SelectAsDTO().FirstAsync();

    public async Task<DTO.User> GetUserByUserNameAsync(string userName)
        => await _DbContext.Users.Where(x => x.UserName == userName && x.Status == true).SelectAsDTO().FirstAsync();

    public async Task<bool> SaveUserAsync(DTO.User userDTO) {
        if (userDTO.Id > 0) {
            var user = await _DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == userDTO.Id).FirstAsync();
            await MapToEntity(userDTO, user);
            _DbContext.Users.Update(user);
        } else {
            var user = new User();
            await MapToEntity(userDTO, user);
            _DbContext.Users.Add(user);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<string> UpdateAccountAsync(DTO.UpdateAccount account) {
        var userName = _HttpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (userName.IsNullOrEmpty()) {
            return Core.ErrorGeneric;
        }

        var user = await _DbContext.Users.FirstAsync(x => x.UserName == userName && x.Status == true);
        user.FirstName = account.FirstName;
        user.LastName = account.LastName;
        user.LanguageId = account.LanguageId;
        user.Email = account.Email;
        _DbContext.Users.Update(user);

        return (await _DbContext.SaveChangesAsync() > 0) ? "" : Core.ErrorGeneric;
    }

    private async Task MapToEntity(DTO.User userDTO, User user) {
        user.Email = userDTO.Email;
        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;
        user.LanguageId = userDTO.LanguageId;
        user.UserName = userDTO.UserName;
        user.Status = true;

        var existingUserRoles = new Dictionary<int, UserRole>();
        if (user.Id > 0 && userDTO.RoleIds?.Any() == true) {
            existingUserRoles = (await _DbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync()).ToDictionary(x => x.RoleId, x => x);
        }

        user.UserRoles = userDTO.RoleIds?.Select(x => {
            if (existingUserRoles.TryGetValue(x, out var userRole)) {
                return userRole;
            }
            return new UserRole { RoleId = x, UserId = userDTO.Id };
        }).ToList() ?? new List<UserRole>();
    }
}
