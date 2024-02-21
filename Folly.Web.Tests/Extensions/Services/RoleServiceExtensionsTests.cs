using Folly.Domain.Models;
using Folly.Extensions.Services;

namespace Folly.Web.Tests.Extensions.Services;

public class RoleServiceExtensionsTests {
    [Fact]
    public void SelectSingleAsDTO_ReturnsProjectedDTO() {
        // arrange
        var rolePermission = new RolePermission { Id = 1, RoleId = 2, PermissionId = 3 };
        var role = new Role { Id = 4, Name = "test role", IsDefault = true, RolePermissions = [rolePermission] };
        var roles = new List<Role> { role }.AsQueryable();

        // act
        var dtos = roles.SelectAsDTO();

        // assert
        Assert.NotNull(dtos);
        Assert.Single(dtos);

        Assert.All(dtos, x => Assert.Equal(role.Id, x.Id));
        Assert.All(dtos, x => Assert.Equal(role.Name, x.Name));
        Assert.All(dtos, x => Assert.Equal(role.IsDefault, x.IsDefault));
        Assert.All(dtos, x => Assert.NotNull(x.PermissionIds));
        Assert.All(dtos, x => Assert.Single(x.PermissionIds!));
        Assert.All(dtos, x => Assert.Contains(rolePermission.PermissionId, x.PermissionIds!));
    }

    [Fact]
    public void SelectMultipleAsDTO_ReturnsProjectedDTOs() {
        // arrange
        var rolePermission1 = new RolePermission { Id = 1, RoleId = 2, PermissionId = 3 };
        var role1 = new Role { Id = 4, Name = "test role 1", IsDefault = true, RolePermissions = [rolePermission1] };
        var rolePermission2 = new RolePermission { Id = 5, RoleId = 6, PermissionId = 7 };
        var role2 = new Role { Id = 6, Name = "test role 2", IsDefault = false, RolePermissions = [rolePermission2] };
        var roles = new List<Role> { role1, role2 }.AsQueryable();

        // act
        var dtos = roles.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(role1.Id, x.Id),
            x => Assert.Equal(role2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(role1.Name, x.Name),
            x => Assert.Equal(role2.Name, x.Name)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(role1.IsDefault, x.IsDefault),
            x => Assert.Equal(role2.IsDefault, x.IsDefault)
        );
        Assert.All(dtos, x => Assert.NotNull(x.PermissionIds));
        Assert.All(dtos, x => Assert.Single(x.PermissionIds!));

        Assert.Collection(dtos,
            x => Assert.Contains(rolePermission1.PermissionId, x.PermissionIds!),
            x => Assert.Contains(rolePermission2.PermissionId, x.PermissionIds!)
        );
    }
}
