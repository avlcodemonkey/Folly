using System.Diagnostics.CodeAnalysis;
using Folly.Constants;
using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions.Services;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class UserService(FollyDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IUserService {
    private readonly FollyDbContext _DbContext = dbContext;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;

    public async Task<bool> DeleteUserAsync(int id) {
        var user = await _DbContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.Status == true);
        if (user == null) {
            return false;
        }

        user.Status = false;
        _DbContext.Users.Update(user);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.User>> GetAllUsersAsync()
        => await _DbContext.Users.Where(x => x.Status == true).SelectAsDTO().ToListAsync();

    public async Task<IEnumerable<DTO.UserClaim>> GetClaimsByUserIdAsync(int id)
        => await _DbContext.UserRoles
            .Include(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                    .ThenInclude(x => x.Permission)
            .Where(x => x.UserId == id)
            .SelectMany(x => x.Role.RolePermissions.Select(y => y.Permission))
            .Select(x => new DTO.UserClaim { Id = x.Id, ActionName = x.ActionName, ControllerName = x.ControllerName })
            .ToListAsync();

    public async Task<DTO.User?> GetUserByIdAsync(int id)
        => await _DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == id && x.Status == true).SelectAsDTO().FirstOrDefaultAsync();

    public async Task<DTO.User?> GetUserByUserNameAsync(string userName)
        => await _DbContext.Users.Where(x => x.UserName == userName && x.Status == true).SelectAsDTO().FirstOrDefaultAsync();

    public async Task<ServiceResult> SaveUserAsync(DTO.User userDTO) {
        if (userDTO.Id > 0) {
            var user = await _DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == userDTO.Id).FirstOrDefaultAsync();
            if (user == null) {
                return ServiceResult.InvalidIdError;
            }

            await MapToEntity(userDTO, user);
            // set the original rowVersion value so concurrency check works correctly
            _DbContext.SetOriginalRowVersion(user, userDTO.RowVersion);

            _DbContext.Users.Update(user);
        } else {
            var user = new User();
            await MapToEntity(userDTO, user);
            _DbContext.Users.Add(user);
        }

        try {
            return await _DbContext.SaveChangesAsync() > 0 ? ServiceResult.Success : ServiceResult.GenericError;
        } catch (DbUpdateConcurrencyException) {
            return ServiceResult.ConcurrencyError;
        }
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
        }).ToList() ?? [];
    }

    public async Task<string> UpdateAccountAsync(DTO.UpdateAccount account) {
        var userName = _HttpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (userName.IsNullOrEmpty()) {
            return Core.ErrorGeneric;
        }

        var user = await _DbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Status == true);
        if (user == null) {
            return Core.ErrorGeneric;
        }

        user.FirstName = account.FirstName;
        user.LastName = account.LastName;
        user.LanguageId = account.LanguageId;
        user.Email = account.Email;
        _DbContext.Users.Update(user);

        return (await _DbContext.SaveChangesAsync() > 0) ? "" : Core.ErrorGeneric;
    }

    [SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons",
        Justification = "Linq can't translate stringComparison methods to sql.")
    ]
    public async Task<IEnumerable<DTO.AutocompleteUser>> FindAutocompleteUsersByNameAsync(string name) {
        var lowerName = (name ?? "").ToLower();
        return await _DbContext.Users
            .Where(x => x.Status == true && (x.FirstName.ToLower().Contains(lowerName) || (x.LastName ?? "").ToLower().Contains(lowerName)))
            .SelectAsAuditLogUserDTO().OrderBy(x => x.Value).ToListAsync();
    }
}
