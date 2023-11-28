using Folly.Domain.Models;
using Folly.Services;
using Folly.Web.Tests.Fixtures;
using DTO = Folly.Models;

namespace Folly.Web.Tests.Services;

[Collection(nameof(DatabaseCollection))]
public class PermissionServiceTests {
    private readonly DatabaseFixture _Fixture;
    private readonly PermissionService _PermissionService;

    public PermissionServiceTests(DatabaseFixture fixture) {
        _Fixture = fixture;
        _PermissionService = new PermissionService(fixture.CreateContext());
    }

    [Fact]
    public async Task GetAllPermissionsAsync_ReturnsManyDTOs() {
        // arrange
        using var context = _Fixture.CreateContext();
        var dashboardPermission = context.Permissions.Find(1)!;
        var totalPermissions = context.Permissions.Count();

        // act
        var permissions = await _PermissionService.GetAllPermissionsAsync();

        // assert
        Assert.NotEmpty(permissions);
        Assert.IsAssignableFrom<IEnumerable<DTO.Permission>>(permissions);
        Assert.Equal(totalPermissions, permissions.Count());
        Assert.Contains(permissions, x => x.Id == dashboardPermission.Id);
    }

    [Fact]
    public async Task SavePermissionAsync_CreatePermission_SavesNewPermission() {
        // arrange
        var createPermission = new DTO.Permission { ControllerName = "save permission controller", ActionName = "save permission action" };

        // act
        var result = await _PermissionService.SavePermissionAsync(createPermission);
        var newPermission = (await _PermissionService.GetAllPermissionsAsync()).FirstOrDefault(x => x.ControllerName == createPermission.ControllerName && x.ActionName == createPermission.ActionName);
        if (newPermission != null) {
            // delete the newly created permission so it doesn't interfere with other tests
            await _PermissionService.DeletePermissionAsync(newPermission.Id);
        }

        // assert
        Assert.True(result);
        Assert.NotNull(newPermission);
        Assert.Equal(createPermission.ControllerName, newPermission.ControllerName);
        Assert.Equal(createPermission.ActionName, newPermission.ActionName);
    }

    [Fact]
    public async Task SavePermissionAsync_UpdatePermission_SavesChanges() {
        // arrange
        var originalController = "original permission controller";
        var originalAction = "original permission action";
        var createPermission = new DTO.Permission { ControllerName = originalController, ActionName = originalAction };
        await _PermissionService.SavePermissionAsync(createPermission);
        var permissionId = (await _PermissionService.GetAllPermissionsAsync()).FirstOrDefault(x => x.ControllerName == originalController && x.ActionName == originalAction)!.Id;
        var newController = "new permission controller";
        var newAction = "new permission action";
        var updatePermission = new DTO.Permission { Id = permissionId, ControllerName = newController, ActionName = newAction };

        // act
        var result = await _PermissionService.SavePermissionAsync(updatePermission);
        var updatedPermission = (await _PermissionService.GetAllPermissionsAsync()).FirstOrDefault(x => x.Id == permissionId);
        if (updatedPermission != null) {
            // delete the newly created permission so it doesn't interfere with other tests
            await _PermissionService.DeletePermissionAsync(updatedPermission.Id);
        }

        // assert
        Assert.True(result);
        Assert.NotNull(updatedPermission);
        Assert.Equal(newController, updatedPermission.ControllerName);
        Assert.Equal(newAction, updatedPermission.ActionName);
    }

    [Fact]
    public async Task SavePermissionAsync_UpdateInvalidPermissionId_ThrowsException() {
        // arrange
        var updatePermission = new DTO.Permission {
            Id = 999, ControllerName = "update permission controller", ActionName = "update permission action"
        };

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _PermissionService.SavePermissionAsync(updatePermission));
    }

    [Fact]
    public async Task DeletePermissionAsync_WithValidPermission_DeletesPermission() {
        // arrange
        var permissionToDelete = new Permission { Id = -100, ControllerName = "delete permission controller", ActionName = "delete permission action" };
        int originalCount;
        using (var dbContext = _Fixture.CreateContext()) {
            dbContext.Permissions.Add(permissionToDelete);
            dbContext.SaveChanges();
            originalCount = dbContext.Permissions.Count();
        }

        // act
        var result = await _PermissionService.DeletePermissionAsync(permissionToDelete.Id);
        var newCount = (await _PermissionService.GetAllPermissionsAsync()).Count();
        var deletedPermission = (await _PermissionService.GetAllPermissionsAsync()).FirstOrDefault(x => x.Id == permissionToDelete.Id);

        // assert
        Assert.True(result);
        Assert.Equal(originalCount - 1, newCount);
        Assert.Null(deletedPermission);
    }

    [Fact]
    public async Task DeletePermissionAsync_WithInvalidPermissionId_ThrowsException() {
        // arrange
        var permissionIdToDelete = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _PermissionService.DeletePermissionAsync(permissionIdToDelete));
    }
}
