using Folly.Constants;
using Folly.Models;

namespace Folly.Services;

public interface IUserService {
    Task<bool> DeleteUserAsync(int id);

    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<IEnumerable<UserClaim>> GetClaimsByUserIdAsync(int id);

    Task<User?> GetUserByIdAsync(int id);

    Task<User?> GetUserByUserNameAsync(string userName);

    Task<ServiceResult> SaveUserAsync(User userDTO);

    Task<string> UpdateAccountAsync(UpdateAccount account);

    Task<IEnumerable<AutocompleteUser>> FindAutocompleteUsersByNameAsync(string name);
}
