using Folly.Domain.Models;
using Folly.Extensions.Services;

namespace Folly.Web.Tests.Extensions.Services;

public class UserServiceExtensionsTests {

    public UserServiceExtensionsTests() { }

    [Fact]
    public void SelectSingleAsDTO_ReturnsProjectedDTO() {
        // arrange
        var userRole = new UserRole { Id = 1, RoleId = 2, UserId = 3 };
        var user = new User {
            Id = 3, Email = "fake@email.com", FirstName = "first name", LastName = "last name", LanguageId = 1,
            UserName = "username", UserRoles = new List<UserRole> { userRole }
        };
        var users = new List<User> { user }.AsQueryable();

        // act
        var dtos = users.SelectAsDTO();

        // assert
        Assert.NotNull(dtos);
        Assert.Single(dtos);

        Assert.All(dtos, x => Assert.Equal(user.Id, x.Id));
        Assert.All(dtos, x => Assert.Equal(user.Email, x.Email));
        Assert.All(dtos, x => Assert.Equal(user.FirstName, x.FirstName));
        Assert.All(dtos, x => Assert.Equal(user.LastName, x.LastName));
        Assert.All(dtos, x => Assert.Equal(user.LanguageId, x.LanguageId));
        Assert.All(dtos, x => Assert.Equal(user.UserName, x.UserName));

        Assert.All(dtos, x => Assert.NotNull(x.RoleIds));
        Assert.All(dtos, x => Assert.Single(x.RoleIds!));
        Assert.All(dtos, x => Assert.Contains(userRole.RoleId, x.RoleIds!));
    }

    [Fact]
    public void SelectMultipleAsDTO_ReturnsProjectedDTOs() {
        // arrange
        var userRole1 = new UserRole { Id = 1, RoleId = 2, UserId = 3 };
        var user1 = new User {
            Id = 3, Email = "fake@email.com", FirstName = "first name", LastName = "last name", LanguageId = 1,
            UserName = "username", UserRoles = new List<UserRole> { userRole1 }
        };
        var userRole2 = new UserRole { Id = 4, RoleId = 5, UserId = 6 };
        var user2 = new User {
            Id = 6, Email = "fake2@email.com", FirstName = "first name 2", LastName = "last name 2", LanguageId = 2,
            UserName = "username2", UserRoles = new List<UserRole> { userRole2 }
        };
        var users = new List<User> { user1, user2 }.AsQueryable();


        // act
        var dtos = users.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(user1.Id, x.Id),
            x => Assert.Equal(user2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(user1.Email, x.Email),
            x => Assert.Equal(user2.Email, x.Email)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(user1.FirstName, x.FirstName),
            x => Assert.Equal(user2.FirstName, x.FirstName)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(user1.LastName, x.LastName),
            x => Assert.Equal(user2.LastName, x.LastName)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(user1.LanguageId, x.LanguageId),
            x => Assert.Equal(user2.LanguageId, x.LanguageId)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(user1.UserName, x.UserName),
            x => Assert.Equal(user2.UserName, x.UserName)
        );

        Assert.All(dtos, x => Assert.NotNull(x.RoleIds));
        Assert.All(dtos, x => Assert.Single(x.RoleIds!));
        Assert.Collection(dtos,
            x => Assert.Contains(userRole1.RoleId, x.RoleIds!),
            x => Assert.Contains(userRole2.RoleId, x.RoleIds!)
        );
    }
}
