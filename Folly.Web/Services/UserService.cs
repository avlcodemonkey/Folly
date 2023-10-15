using Folly.Domain.Models;
using Folly.Resources;
using Microsoft.EntityFrameworkCore;
using DTO = Folly.Models;

namespace Folly.Services;

public sealed class UserService : IUserService {
    private readonly FollyDbContext DbContext;
    private readonly IHttpContextAccessor HttpContextAccessor;
    private readonly IRoleService RoleService;

    private static DTO.User MapUserToDTO(User user) => new() {
        Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName,
        LanguageId = user.LanguageId, UserName = user.UserName,
        RoleIds = user.UserRoles.Select(x => x.RoleId)
    };

    private async Task<User?> CurrentUser() {
        var userName = HttpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (userName.IsEmpty())
            return null;

        return await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Status == true);
    }

    private async Task<User> MapUserToModel(DTO.User userDTO) {
        User model;
        if (userDTO.IsCreate) {
            model = new() {
                Id = userDTO.Id, Email = userDTO.Email, FirstName = userDTO.FirstName, LastName = userDTO.LastName,
                LanguageId = userDTO.LanguageId, UserName = userDTO.UserName, Status = true
            };
        } else {
            model = await DbContext.Users.Include(x => x.UserRoles).FirstAsync(x => x.Id == userDTO.Id);
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
        DbContext = dbContext;
        HttpContextAccessor = httpContextAccessor;
        RoleService = roleService;
    }

    public async Task<bool> AddUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        var defaultRole = await RoleService.GetDefaultRole();
        if (user.UserRoles?.Any() != true && defaultRole != null)
            user.UserRoles = new List<UserRole> { new() { RoleId = defaultRole.Id } };
        DbContext.Users.Add(user);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        user.Status = false;
        DbContext.Users.Update(user);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<DTO.User>> GetAllUsers() => await DbContext.Users.Where(x => x.Status == true).Select(x => MapUserToDTO(x)).ToListAsync();

    public async Task<IEnumerable<DTO.UserClaim>> GetClaimsByUserId(int id) => await DbContext.UserRoles
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
        => await DbContext.Users.Include(x => x.UserRoles).Where(x => x.Id == id && x.Status == true).Select(x => MapUserToDTO(x)).FirstAsync();

    public async Task<DTO.User> GetUserByUserName(string userName)
        => await DbContext.Users.Where(x => x.UserName == userName && x.Status == true).Select(x => MapUserToDTO(x)).FirstAsync();

    public async Task<bool> SaveUser(DTO.User userDTO) {
        var user = await MapUserToModel(userDTO);
        if (user.Id > 0) {
            var existingRoles = await DbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            DbContext.UserRoles.RemoveRange(existingRoles.Where(x => !user.UserRoles.Any(y => y.RoleId == x.RoleId)));
            DbContext.Users.Update(user);
        } else {
            DbContext.Users.Add(user);
        }

        return await DbContext.SaveChangesAsync() > 0;
    }

    public async Task<string> UpdateProfile(DTO.UpdateProfile profile) {
        // load user object and copy settings the user is allowed to change
        var user = await CurrentUser();
        if (user == null)
            return Core.ErrorGeneric;

        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;
        user.LanguageId = profile.LanguageId;
        user.Email = profile.Email;
        DbContext.Users.Update(user);

        return (await DbContext.SaveChangesAsync() > 0) ? "" : Core.ErrorGeneric;
    }
}
