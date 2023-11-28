using Folly.Domain.Models;
using Folly.Extensions.Services;

namespace Folly.Web.Tests.Extensions.Services;

public class PermissionServiceExtensionsTests {

    public PermissionServiceExtensionsTests() { }

    [Fact]
    public void SelectSingleAsDTO_ReturnsProjectedDTO() {
        // arrange
        var permission = new Permission { Id = 1, ControllerName = "controller", ActionName = "action" };
        var permissions = new List<Permission> { permission }.AsQueryable();

        // act
        var dtos = permissions.SelectAsDTO();

        // assert
        Assert.NotNull(dtos);
        Assert.Single(dtos);

        Assert.All(dtos, x => Assert.Equal(permission.Id, x.Id));
        Assert.All(dtos, x => Assert.Equal(permission.ControllerName, x.ControllerName));
        Assert.All(dtos, x => Assert.Equal(permission.ActionName, x.ActionName));
    }

    [Fact]
    public void SelectMultipleAsDTO_ReturnsProjectedDTOs() {
        // arrange
        var permission1 = new Permission { Id = 1, ControllerName = "controller 1", ActionName = "action 1" };
        var permission2 = new Permission { Id = 2, ControllerName = "controller 2", ActionName = "action 2" };
        var roles = new List<Permission> { permission1, permission2 }.AsQueryable();

        // act
        var dtos = roles.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(permission1.Id, x.Id),
            x => Assert.Equal(permission2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(permission1.ControllerName, x.ControllerName),
            x => Assert.Equal(permission2.ControllerName, x.ControllerName)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(permission1.ActionName, x.ActionName),
            x => Assert.Equal(permission2.ActionName, x.ActionName)
        );
    }
}
