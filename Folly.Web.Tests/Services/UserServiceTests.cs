using System.Security.Principal;
using Folly.Constants;
using Folly.Domain.Models;
using Folly.Services;
using Folly.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Moq;
using DTO = Folly.Models;

namespace Folly.Web.Tests.Services;

[Collection(nameof(DatabaseCollection))]
public class UserServiceTests {
    private readonly DatabaseFixture _Fixture;
    private readonly Mock<IHttpContextAccessor> _MockHttpContextAccessor;
    private readonly UserService _UserService;

    private UserService GetNewUserService() => new(_Fixture.CreateContext(), _MockHttpContextAccessor.Object);

    /// <summary>
    /// Delete a user so it doesn't interfere with other tests.
    /// </summary>
    private async Task DeleteUserIfExistsAsync(int userId) {
        var user = (await GetNewUserService().GetAllUsersAsync()).FirstOrDefault(x => x.Id == userId);
        if (user != null) {
            // need a new context for this to avoid concurrency error
            await GetNewUserService().DeleteUserAsync(user.Id);
        }
    }

    public UserServiceTests(DatabaseFixture fixture) {
        _Fixture = fixture;

        _MockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _MockHttpContextAccessor.Setup(x => x.HttpContext!.User.Identity).Returns(new GenericIdentity(fixture.TestUser.UserName, "test"));

        _UserService = GetNewUserService();
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsTestUserDTO() {
        // arrange
        var testUser = _Fixture.TestUser;

        // act
        var user = await _UserService.GetUserByIdAsync(testUser.Id);

        // assert
        Assert.NotNull(user);
        Assert.IsType<DTO.User>(user);
        Assert.Equal(testUser.FirstName, user.FirstName);
        Assert.Equal(testUser.LastName, user.LastName);
        Assert.Equal(testUser.Email, user.Email);
        Assert.Equal(testUser.UserName, user.UserName);
        Assert.Equal(testUser.LanguageId, user.LanguageId);

        Assert.NotNull(user.RoleIds);
        Assert.Equal(testUser.UserRoles.Count(), user.RoleIds.Count());
        Assert.Equal(testUser.UserRoles.First().RoleId, user.RoleIds.First());
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidUserId_ReturnsNull() {
        // arrange
        var userIdToGet = -200;

        // act
        var result = await _UserService.GetUserByIdAsync(userIdToGet);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_ReturnsTestUserDTO() {
        // arrange
        var testUser = _Fixture.TestUser;

        // act
        var user = await _UserService.GetUserByUserNameAsync(testUser.UserName);

        // assert
        Assert.NotNull(user);
        Assert.IsType<DTO.User>(user);
        Assert.Equal(testUser.FirstName, user.FirstName);
        Assert.Equal(testUser.LastName, user.LastName);
        Assert.Equal(testUser.Email, user.Email);
        Assert.Equal(testUser.UserName, user.UserName);
        Assert.Equal(testUser.LanguageId, user.LanguageId);

        Assert.NotNull(user.RoleIds);
        Assert.Equal(testUser.UserRoles.Count(), user.RoleIds.Count());
        Assert.Equal(testUser.UserRoles.First().RoleId, user.RoleIds.First());
    }

    [Fact]
    public async Task GetUserByUserNameAsync_WithInvalidUserName_ReturnsNull() {
        // arrange
        var userNameToGet = "fakeUser";

        // act
        var result = await _UserService.GetUserByUserNameAsync(userNameToGet);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsUserDTOs() {
        // arrange
        var user = _Fixture.User;
        var testUser = _Fixture.TestUser;

        // act
        var users = await _UserService.GetAllUsersAsync();

        // assert
        Assert.NotEmpty(users);
        Assert.IsAssignableFrom<IEnumerable<DTO.User>>(users);
        Assert.Equal(3, users.Count());
        Assert.Collection(users,
            x => Assert.Equal(testUser.Id, x.Id),
            x => Assert.Equal(user.Id, x.Id),
            x => Assert.Equal(1, x.Id)
        );
    }

    [Fact]
    public async Task SaveUserAsync_CreateUser_SavesNewUser() {
        // arrange
        var testRole = _Fixture.TestRole;
        var createUser = new DTO.User {
            UserName = "createTest", FirstName = "create first", LastName = "create last", Email = "create@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };

        // act
        var result = await _UserService.SaveUserAsync(createUser);
        var newUser = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == createUser.UserName);
        if (newUser != null) {
            // delete the newly created user so it doesn't interfere with other tests
            await GetNewUserService().DeleteUserAsync(newUser.Id);
        }

        // assert
        Assert.Equal(ServiceResult.Success, result);
        Assert.NotNull(newUser);
        Assert.NotNull(newUser.RoleIds);
        Assert.Equal(createUser.RoleIds.Count(), newUser.RoleIds.Count());
    }

    [Fact]
    public async Task SaveUserAsync_UpdateUser_SavesChanges() {
        // arrange
        var testRole = _Fixture.TestRole;
        var originalUserName = "originalUserName";
        var createUser = new DTO.User {
            UserName = originalUserName, FirstName = "original first", LastName = "original last", Email = "update@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };
        await _UserService.SaveUserAsync(createUser);
        var userId = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == originalUserName)!.Id;
        var newUserName = "newUserName";
        var updateUser = new DTO.User {
            Id = userId, UserName = newUserName, FirstName = "new first", LastName = "new last", Email = "update@email.com",
            LanguageId = 1, RoleIds = []
        };

        // act
        var result = await _UserService.SaveUserAsync(updateUser);
        var updatedUser = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.Id == userId);
        if (updatedUser != null) {
            // delete the newly created user so it doesn't interfere with other tests
            await GetNewUserService().DeleteUserAsync(updatedUser.Id);
        }

        // assert
        Assert.Equal(ServiceResult.Success, result);
        Assert.NotNull(updatedUser);
        Assert.Equal(newUserName, updatedUser.UserName);
        Assert.NotEqual(createUser.FirstName, updatedUser.FirstName);
        Assert.NotEqual(createUser.LastName, updatedUser.LastName);
        Assert.Equal(createUser.Email, updatedUser.Email);
        Assert.Equal(createUser.LanguageId, updatedUser.LanguageId);
        Assert.NotNull(updatedUser.RoleIds);
        Assert.Empty(updatedUser.RoleIds);
    }

    [Fact]
    public async Task SaveUserAsync_UpdateInvalidUserId_ReturnsFalse() {
        // arrange
        var updateUser = new DTO.User {
            Id = 999, UserName = "updateUserName", FirstName = "update first", LastName = "update last", Email = "update@email.com",
            LanguageId = 1, RoleIds = []
        };

        // act
        var result = await _UserService.SaveUserAsync(updateUser);

        // assert
        Assert.Equal(ServiceResult.InvalidIdError, result);
    }

    [Fact]
    public async Task SaveUserAsync_WithConcurrentChanges_ReturnsConcurrencyError() {
        // arrange
        var testRole = _Fixture.TestRole;
        var originalUserName = "concurrency1 userName";
        var createUser = new DTO.User {
            UserName = originalUserName, FirstName = "concurrency1 first", LastName = "concurrency1 last", Email = "concurrency1@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };
        await _UserService.SaveUserAsync(createUser);

        var userCopy = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == originalUserName);
        var updateUser = userCopy! with { UserName = "concurrency1 new userName" };
        var finalUser = userCopy with { UserName = "concurrency1 final userName" };

        // act
        var result = await _UserService.SaveUserAsync(updateUser);
        var result2 = await _UserService.SaveUserAsync(finalUser);

        await DeleteUserIfExistsAsync(userCopy.Id);

        // assert
        Assert.Equal(ServiceResult.Success, result);
        Assert.Equal(ServiceResult.ConcurrencyError, result2);
    }

    [Fact]
    public async Task SaveUserAsync_WithSameRowVersionAndConcurrentChanges_ReturnsConcurrencyError() {
        // arrange
        var testRole = _Fixture.TestRole;
        var originalUserName = "concurrency2 userName";
        var createUser = new DTO.User {
            UserName = originalUserName, FirstName = "concurrency2 first", LastName = "concurrency2 last", Email = "concurrency2@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };
        await _UserService.SaveUserAsync(createUser);

        var userCopy = (await _UserService.GetAllUsersAsync()).First(x => x.UserName == originalUserName);
        var updateUser = userCopy with { UserName = "concurrency2 new userName", RowVersion = 0 };
        var finalUser = userCopy with { UserName = "concurrency2 final userName", RowVersion = 0 };

        // act
        var result = await GetNewUserService().SaveUserAsync(updateUser);
        var result2 = await GetNewUserService().SaveUserAsync(finalUser);

        await DeleteUserIfExistsAsync(userCopy.Id);

        // assert
        Assert.Equal(ServiceResult.Success, result);
        Assert.Equal(ServiceResult.ConcurrencyError, result2);
    }

    [Fact]
    public async Task SaveUserAsync_WithIncrementedRowVersionAndConcurrentChanges_ReturnsSuccess() {
        // arrange
        var testRole = _Fixture.TestRole;
        var originalUserName = "concurrency3 userName";
        var createUser = new DTO.User {
            UserName = originalUserName, FirstName = "concurrency3 first", LastName = "concurrency3 last", Email = "concurrency3@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };
        await _UserService.SaveUserAsync(createUser);

        var userCopy = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == originalUserName);
        var updateUser = userCopy! with { UserName = "concurrency3 new userName", RowVersion = 0 };
        var finalUser = userCopy with { UserName = "concurrency3 final userName", RowVersion = 1 };

        // act
        var result = await _UserService.SaveUserAsync(updateUser);
        var result2 = await _UserService.SaveUserAsync(finalUser);

        await DeleteUserIfExistsAsync(userCopy.Id);

        // assert
        Assert.Equal(ServiceResult.Success, result);
        Assert.Equal(ServiceResult.Success, result2);
    }

    [Fact]
    public async Task DeleteUserAsync_WithValidUser_DeletesUser() {
        // arrange
        var userToDelete = new User {
            Id = -100, UserName = "deleteTest", FirstName = "delete first", LastName = "delete last",
            Email = "delete@email.com", LanguageId = 1
        };
        using (var dbContext = _Fixture.CreateContext()) {
            dbContext.Users.Add(userToDelete);
            dbContext.SaveChanges();
        }

        // act
        var result = await _UserService.DeleteUserAsync(userToDelete.Id);
        var deletedUser = _Fixture.CreateContext().Users.FirstOrDefault(x => x.Id == userToDelete.Id);

        // assert
        Assert.True(result);
        Assert.NotNull(deletedUser);
        Assert.False(deletedUser.Status);
    }

    [Fact]
    public async Task DeleteUserAsync_WithInvalidUserId_ReturnsFalse() {
        // arrange
        var userIdToDelete = -200;

        // act
        var result = await _UserService.DeleteUserAsync(userIdToDelete);

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetClaimsByUserIdAsync_WithValidUserId_ReturnsUserClaims() {
        // arrange
        var testUser = _Fixture.TestUser;
        var testPermission = _Fixture.TestPermission;

        // act
        var claims = await _UserService.GetClaimsByUserIdAsync(testUser.Id);

        // assert
        Assert.NotEmpty(claims);
        Assert.IsAssignableFrom<IEnumerable<DTO.UserClaim>>(claims);
        var x = Assert.Single(claims);
        Assert.Equal(testPermission.ControllerName, x.ControllerName);
        Assert.Equal(testPermission.ActionName, x.ActionName);
    }

    [Fact]
    public async Task GetClaimsByUserIdAsync_WithInvalidUserId_ReturnsEmptyList() {
        // arrange
        var userId = -300;

        // act
        var claims = await _UserService.GetClaimsByUserIdAsync(userId);

        // assert
        Assert.Empty(claims);
        Assert.IsAssignableFrom<IEnumerable<DTO.UserClaim>>(claims);
    }

    [Fact]
    public async Task UpdateAccountAsync_WithInvalidUserId_ThrowsException() {
        // arrange
        var testRole = _Fixture.TestRole;

        // first create a new user in the db
        var originalUserName = "updateAccountUserName";
        var updateAccountUser = new DTO.User {
            UserName = originalUserName, FirstName = "update account first", LastName = "update account last", Email = "updateAccount@email.com",
            LanguageId = 1, RoleIds = [testRole.Id]
        };
        await _UserService.SaveUserAsync(updateAccountUser);

        // now load the new user from the db so we have their userId
        var user = _Fixture.CreateContext().Users.FirstOrDefault(x => x.UserName == originalUserName)!;

        // now create the UpdateAccount record
        var newEmail = "new@email.com";
        var newFirstName = "new first";
        var newLastName = "new last";
        var newLanguageId = 2;
        var updateAccount = new DTO.UpdateAccount {
            Email = newEmail, FirstName = newFirstName, LastName = newLastName, LanguageId = newLanguageId
        };

        // create a new userService that will act like the newly created user
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(x => x.HttpContext!.User.Identity).Returns(new GenericIdentity(originalUserName, "test"));
        var userService = new UserService(_Fixture.CreateContext(user), mockHttpContextAccessor.Object);

        // act
        var result = await userService.UpdateAccountAsync(updateAccount);
        var updatedUser = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.Id == user.Id);
        if (updatedUser != null) {
            // delete the newly created user so it doesn't interfere with other tests
            // need a new context for this to avoid concurrency error
            await GetNewUserService().DeleteUserAsync(updatedUser.Id);
        }

        // assert
        Assert.Empty(result);
        Assert.NotNull(updatedUser);
        Assert.Equal(newFirstName, updatedUser.FirstName);
        Assert.Equal(newLastName, updatedUser.LastName);
        Assert.Equal(newEmail, updatedUser.Email);
        Assert.Equal(newLanguageId, updatedUser.LanguageId);
    }

    [Fact]
    public async Task FindAutocompleteUsersByNameAsync_WithEmptyQuery_ReturnsAllUserDTOs() {
        // arrange
        var user = _Fixture.User;
        var testUser = _Fixture.TestUser;
        var totalUsers = _Fixture.CreateContext().Users.Where(x => x.Status == true).Count();

        // act
        var users = await _UserService.FindAutocompleteUsersByNameAsync("");

        // assert
        Assert.NotEmpty(users);
        Assert.IsAssignableFrom<IEnumerable<DTO.AutocompleteUser>>(users);
        Assert.Equal(totalUsers, users.Count());
        Assert.Collection(users,
            x => Assert.Equal(testUser.Id, x.Value),   // testUser from fixture
            x => Assert.Equal(user.Id, x.Value),       // user from fixture
            x => Assert.Equal(1, x.Value)              // admin user from original db migration
        );
    }

    [Fact]
    public async Task FindAutocompleteUsersByNameAsync_WithUserQuery_ReturnsMatchingDTO() {
        // arrange
        var user = _Fixture.User;

        // act
        var users = await _UserService.FindAutocompleteUsersByNameAsync(user.UserName);

        // assert
        Assert.NotEmpty(users);
        Assert.IsAssignableFrom<IEnumerable<DTO.AutocompleteUser>>(users);
        var resultUser = Assert.Single(users);
        Assert.Equal(user.Id, resultUser.Value);
    }

    [Fact]
    public async Task FindAutocompleteUsersByNameAsync_WithGibberishQuery_ReturnsNoMatches() {
        // arrange

        // act
        var users = await _UserService.FindAutocompleteUsersByNameAsync("gibberish");

        // assert
        Assert.Empty(users);
    }
}
