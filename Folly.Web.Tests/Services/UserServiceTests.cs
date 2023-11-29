using System.Security.Principal;
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
    private readonly UserService _UserService;

    public UserServiceTests(DatabaseFixture fixture) {
        _Fixture = fixture;

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(x => x.HttpContext!.User.Identity).Returns(new GenericIdentity(fixture.TestUser.UserName, "test"));

        _UserService = new UserService(fixture.CreateContext(), mockHttpContextAccessor.Object);
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
    public async Task GetUserByIdAsync_WithInvalidUserId_ThrowsException() {
        // arrange
        var userIdToGet = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _UserService.GetUserByIdAsync(userIdToGet));
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
    public async Task GetUserByUserNameAsync_WithInvalidUserName_ThrowsException() {
        // arrange
        var userNameToGet = "fakeUser";

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _UserService.GetUserByUserNameAsync(userNameToGet));
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsTwoUserDTOs() {
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
            LanguageId = 1, RoleIds = new[] { testRole.Id }
        };

        // act
        var result = await _UserService.SaveUserAsync(createUser);
        var newUser = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == createUser.UserName);
        if (newUser != null) {
            // delete the newly created user so it doesn't interfere with other tests
            await _UserService.DeleteUserAsync(newUser.Id);
        }

        // assert
        Assert.True(result);
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
            LanguageId = 1, RoleIds = new[] { testRole.Id }
        };
        await _UserService.SaveUserAsync(createUser);
        var userId = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.UserName == originalUserName)!.Id;
        var newUserName = "newUserName";
        var updateUser = new DTO.User {
            Id = userId, UserName = newUserName, FirstName = "new first", LastName = "new last", Email = "update@email.com",
            LanguageId = 1, RoleIds = Array.Empty<int>()
        };

        // act
        var result = await _UserService.SaveUserAsync(updateUser);
        var updatedUser = (await _UserService.GetAllUsersAsync()).FirstOrDefault(x => x.Id == userId);
        if (updatedUser != null) {
            // delete the newly created user so it doesn't interfere with other tests
            await _UserService.DeleteUserAsync(updatedUser.Id);
        }

        // assert
        Assert.True(result);
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
    public async Task SaveUserAsync_UpdateInvalidUserId_ThrowsException() {
        // arrange
        var updateUser = new DTO.User {
            Id = 999, UserName = "updateUserName", FirstName = "update first", LastName = "update last", Email = "update@email.com",
            LanguageId = 1, RoleIds = Array.Empty<int>()
        };

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _UserService.SaveUserAsync(updateUser));
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
    public async Task DeleteUserAsync_WithInvalidUserId_ThrowsException() {
        // arrange
        var userIdToDelete = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _UserService.DeleteUserAsync(userIdToDelete));
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
        Assert.Single(claims);
        Assert.Collection(claims,
            x => Assert.Equal(testPermission.ControllerName, x.ControllerName)
        );
        Assert.Collection(claims,
            x => Assert.Equal(testPermission.ActionName, x.ActionName)
        );
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
            LanguageId = 1, RoleIds = new[] { testRole.Id }
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
            await _UserService.DeleteUserAsync(updatedUser.Id);
        }

        // assert
        Assert.Empty(result);
        Assert.NotNull(updatedUser);
        Assert.Equal(newFirstName, updatedUser.FirstName);
        Assert.Equal(newLastName, updatedUser.LastName);
        Assert.Equal(newEmail, updatedUser.Email);
        Assert.Equal(newLanguageId, updatedUser.LanguageId);
    }
}
