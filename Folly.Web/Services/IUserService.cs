using Folly.Models;

namespace Folly.Services;

public interface IUserService {
    Task<bool> AddUser(User userDTO);

    Task<bool> DeleteUser(User userDTO);

    Task<IEnumerable<User>> GetAllUsers();

    Task<IEnumerable<UserClaim>> GetClaimsByUserId(int id);

    Task<User?> GetCurrentUser();

    Task<User> GetUserById(int id);

    Task<User> GetUserByUserName(string userName);

    Task<bool> SaveUser(User userDTO);

    Task<string> UpdateAccount(UpdateAccount account);
}
