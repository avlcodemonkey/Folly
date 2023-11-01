using Folly.Domain;
using Folly.Domain.Models;
using Folly.Extensions;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class UserService : IUserService {
    private readonly FollyDbContext _DbContext;
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly IRoleService _RoleService;

    private static DTO.User MapUserToDTO(User user) => new() {
        Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName,
        LanguageId = user.LanguageId, UserName = user.UserName,
        RoleIds = user.UserRoles.Select(x => x.RoleId)
    };

    private async Task<User?> CurrentUser() {
        var userName = _HttpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (userName.IsEmpty())
            return null;

        return await _DbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Status == true);
    }

    private async Task<User> MapUserToModel(DTO.User userDTO) {
        User model;
        if (userDTO.IsCreate) {
            model = new() {
                Id = userDTO.Id, Email = userDTO.Email, FirstName = userDTO.FirstName, LastName = userDTO.LastName,
                LanguageId = userDTO.LanguageId, UserName = userDTO.UserName, Status = true
            };
        } else {
            model = await _DbContext.Users.Include(x => x.UserRoles).FirstAsync(x => x.Id == userDTO.Id);
            model.Email = userDTO.Email;
            model.FirstName = userDTO.FirstName;
            model.LastName = userDTO.LastName;
            model.LanguageId = userDTO.LanguageId;
            model.UserName = userDTO.UserName;
        }

        // get the existing user roles that we are keeping
        var roles = model.UserRoles.Where(x => userDTO.RoleIds?.Contains(x.RoleId) == true).ToList();
        // get the new user roles we are adding
        var newRoles = userDTO.RoleIds?.Where(x => !roles.Any(y => y.RoleId == x))
            .Select(x => new UserRole { RoleId = x, UserId = model.Id });
        if (newRoles?.Any() == true)
            roles.AddRange(newRoles);
        model.UserRoles = roles;

        return model;
    }

    public UserService(FollyDbContext dbContext, IHttpContextAccessor httpContextAccessor, IRoleService roleService) {
        _DbContext = dbContext;
        _HttpContextAccessor = httpContextAccessor;
        _RoleService = roleService;
    }

    public async Task<bool> AddUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        var defaultRole = await _RoleService.GetDefaultRoleAsync();
        if (user.UserRoles?.Any() != true && defaultRole != null)
            user.UserRoles = new List<UserRole> { new() { RoleId = defaultRole.Id } };
        _DbContext.Users.Add(user);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        user.Status = false;
        _DbContext.Users.Update(user);
        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.User>> GetAllUsers() => await _DbContext.Users.Where(x => x.Status == true).Select(x => MapUserToDTO(x)).ToListAsync();

    public async Task<IEnumerable<DTO.UserClaim>> GetClaimsByUserId(int id) => await _DbContext.UserRoles
        .Include(x => x.Role)
            .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
        .Where(x => x.UserId == id)
        .SelectMany(x => x.Role.RolePermissions.Select(y => y.Permission))
        .Select(x => new DTO.UserClaim { Id = x.Id, ActionName = x.ActionName, ControllerName = x.ControllerName })
        .ToListAsync();

    public async Task<DTO.User?> GetCurrentUser() {
        var user = await CurrentUser();
        return user == null ? null : MapUserToDTO(user);
    }

    public async Task<DTO.User> GetUserById(int id)
        => await _DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == id && x.Status == true).Select(x => MapUserToDTO(x)).FirstAsync();

    public async Task<DTO.User> GetUserByUserName(string userName)
        => await _DbContext.Users.Where(x => x.UserName == userName && x.Status == true).Select(x => MapUserToDTO(x)).FirstAsync();

    public async Task<bool> SaveUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        if (user.Id > 0) {
            var existingRoles = await _DbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            _DbContext.UserRoles.RemoveRange(existingRoles.Where(x => !user.UserRoles.Any(y => y.RoleId == x.RoleId)));
            _DbContext.Users.Update(user);
        } else {
            _DbContext.Users.Add(user);
        }

        return await _DbContext.SaveChangesAsync() > 0;
    }

    public async Task<string> UpdateAccount(DTO.UpdateAccount account) {
        // load user object and copy settings the user is allowed to change
        var user = await CurrentUser();
        if (user == null)
            return Core.ErrorGeneric;

        user.FirstName = account.FirstName;
        user.LastName = account.LastName;
        user.LanguageId = account.LanguageId;
        user.Email = account.Email;
        _DbContext.Users.Update(user);

        return (await _DbContext.SaveChangesAsync() > 0) ? "" : Core.ErrorGeneric;
    }
}
