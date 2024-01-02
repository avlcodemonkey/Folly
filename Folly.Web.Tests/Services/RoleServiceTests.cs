using Folly.Domain.Models;
using Folly.Services;
using Folly.Web.Tests.Fixtures;
using DTO = Folly.Models;

namespace Folly.Web.Tests.Services;

[Collection(nameof(DatabaseCollection))]
public class RoleServiceTests(DatabaseFixture fixture) {
    private readonly DatabaseFixture _Fixture = fixture;
    private readonly RoleService _RoleService = new(fixture.CreateContext());

    [Fact]
    public async Task GetDefaultRoleAsync_ReturnsAdminRoleDTO() {
        // arrange - no need to do anything since the model builder creates a default role

        // act
        var defaultRole = await _RoleService.GetDefaultRoleAsync();

        // assert
        Assert.NotNull(defaultRole);
        Assert.IsType<DTO.Role>(defaultRole);
        Assert.Equal("Administrator", defaultRole.Name);
        Assert.True(defaultRole.IsDefault);
    }

    [Fact]
    public async Task GetRoleByIdAsync_ReturnsTestRoleDTO() {
        // arrange
        var testRole = _Fixture.TestRole;

        // act
        var role = await _RoleService.GetRoleByIdAsync(testRole.Id);

        // assert
        Assert.NotNull(role);
        Assert.IsType<DTO.Role>(role);
        Assert.Equal(testRole.Name, role.Name);
        Assert.False(role.IsDefault);
        Assert.NotNull(role.PermissionIds);
        Assert.Equal(testRole.RolePermissions.Count, role.PermissionIds.Count());
        Assert.Equal(testRole.RolePermissions.First().PermissionId, role.PermissionIds.First());
    }

    [Fact]
    public async Task GetRoleByIdAsync_WithInvalidRoleId_ThrowsException() {
        // arrange
        var roleIdToGet = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _RoleService.GetRoleByIdAsync(roleIdToGet));
    }

    [Fact]
    public async Task GetAllRolesAsync_ReturnsTwoRoleDTOs() {
        // arrange
        var testRole = _Fixture.TestRole;

        // act
        var roles = await _RoleService.GetAllRolesAsync();

        // assert
        Assert.NotEmpty(roles);
        Assert.IsAssignableFrom<IEnumerable<DTO.Role>>(roles);
        Assert.Equal(2, roles.Count());
        Assert.Collection(roles,
            x => Assert.Equal(testRole.Id, x.Id),
            x => Assert.Equal(1, x.Id)
        );
    }

    [Fact]
    public async Task SaveRoleAsync_CreateRole_SavesNewRole() {
        // arrange
        var testPermission = _Fixture.TestPermission;
        var createRole = new DTO.Role {
            Name = "Create test", IsDefault = false,
            PermissionIds = new[] { testPermission.Id }
        };

        // act
        var result = await _RoleService.SaveRoleAsync(createRole);
        var newRole = (await _RoleService.GetAllRolesAsync()).FirstOrDefault(x => x.Name == createRole.Name);
        if (newRole != null) {
            // delete the newly created role so it doesn't interfere with other tests
            await _RoleService.DeleteRoleAsync(newRole.Id);
        }

        // assert
        Assert.True(result);
        Assert.NotNull(newRole);
        Assert.NotNull(newRole.PermissionIds);
        Assert.Equal(createRole.PermissionIds.Count(), newRole.PermissionIds.Count());
    }

    [Fact]
    public async Task SaveRoleAsync_UpdateRole_SavesChanges() {
        // arrange
        var testPermission = _Fixture.TestPermission;
        var originalName = "original name";
        var createRole = new DTO.Role {
            Name = originalName, IsDefault = false,
            PermissionIds = new[] { testPermission.Id }
        };
        await _RoleService.SaveRoleAsync(createRole);
        var roleId = (await _RoleService.GetAllRolesAsync()).FirstOrDefault(x => x.Name == originalName)!.Id;
        var newRoleName = "new name";
        var updateRole = new DTO.Role {
            Id = roleId, Name = newRoleName, IsDefault = false,
            PermissionIds = Array.Empty<int>()
        };

        // act
        var result = await _RoleService.SaveRoleAsync(updateRole);
        var updatedRole = (await _RoleService.GetAllRolesAsync()).FirstOrDefault(x => x.Id == roleId);
        if (updatedRole != null) {
            // delete the newly created role so it doesn't interfere with other tests
            await _RoleService.DeleteRoleAsync(updatedRole.Id);
        }

        // assert
        Assert.True(result);
        Assert.NotNull(updatedRole);
        Assert.Equal(newRoleName, updatedRole.Name);
        Assert.NotNull(updatedRole.PermissionIds);
        Assert.Empty(updatedRole.PermissionIds);
    }

    [Fact]
    public async Task SaveRoleAsync_UpdateInvalidRoleId_ThrowsException() {
        // arrange
        var updateRole = new DTO.Role {
            Id = 999, Name = "Update role", IsDefault = false,
            PermissionIds = Array.Empty<int>()
        };

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _RoleService.SaveRoleAsync(updateRole));
    }

    [Fact]
    public async Task CopyRoleAsync_WithValidRole_SavesNewRole() {
        // arrange
        var testRole = _Fixture.TestRole;
        var copyRole = new DTO.CopyRole { Id = testRole.Id, Prompt = "Copy of test role" };

        // act
        var result = await _RoleService.CopyRoleAsync(copyRole);
        var newRole = (await _RoleService.GetAllRolesAsync()).FirstOrDefault(x => x.Name == copyRole.Prompt);
        if (newRole != null) {
            // delete the copy so it doesn't interfere with other tests
            await _RoleService.DeleteRoleAsync(newRole.Id);
        }

        // assert
        Assert.True(result);
        Assert.NotNull(newRole);
        Assert.Equal(copyRole.Prompt, newRole.Name);
        Assert.NotNull(newRole.PermissionIds);
        Assert.Equal(testRole.RolePermissions.Count, newRole.PermissionIds.Count());
    }

    [Fact]
    public async Task CopyRoleAsync_WithInvalidRoleId_ThrowsException() {
        // arrange
        var copyRole = new DTO.CopyRole { Id = -300, Prompt = "Copy" };

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _RoleService.CopyRoleAsync(copyRole));
    }

    [Fact]
    public async Task DeleteRoleAsync_WithValidRole_DeletesRole() {
        // arrange
        var roleToDelete = new Role { Id = -100, Name = "Delete Test", IsDefault = false };
        int originalCount;
        using (var dbContext = _Fixture.CreateContext()) {
            dbContext.Roles.Add(roleToDelete);
            dbContext.SaveChanges();
            originalCount = dbContext.Roles.Count();
        }

        // act
        var result = await _RoleService.DeleteRoleAsync(roleToDelete.Id);
        var newCount = (await _RoleService.GetAllRolesAsync()).Count();

        // assert
        Assert.True(result);
        Assert.Equal(originalCount - 1, newCount);
    }

    [Fact]
    public async Task DeleteRoleAsync_WithInvalidRoleId_ThrowsException() {
        // arrange
        var roleIdToDelete = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _RoleService.DeleteRoleAsync(roleIdToDelete));
    }
}
