using Folly.Models;
using Folly.Services;
using Folly.Utils;
using Moq;

namespace Folly.Web.Tests.Utils;

public class PermissionManagerTests {
    private readonly Dictionary<string, string> _ActionList = new() {
        { "controller1.action1", "Controller1.Action1" },
        { "controller1.action2", "Controller1.Action2" },
        { "controller2.action1", "Controller2.Action1" },
    };

    private readonly List<Permission> _PermissionList = [
        new Permission { ControllerName = "controller1", ActionName = "action1" },
        new Permission { ControllerName = "controller1", ActionName = "action2" },
        new Permission { ControllerName = "controller2", ActionName = "action1" },
    ];

    private readonly Role _Role = new() { Name = "test role" };

    [Fact]
    public async Task Register_WithNoChanges_DoesNothing() {
        // arrange
        var mockAssemblyService = new Mock<IAssemblyService>();
        mockAssemblyService.Setup(x => x.GetActionList()).Returns(_ActionList);

        var mockPermissionService = new Mock<IPermissionService>();
        mockPermissionService.Setup(x => x.GetAllPermissionsAsync()).ReturnsAsync(_PermissionList);
        mockPermissionService.Setup(x => x.SavePermissionAsync(It.IsAny<Permission>())).ReturnsAsync(true);
        mockPermissionService.Setup(x => x.DeletePermissionAsync(It.IsAny<int>())).ReturnsAsync(true);

        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);

        var permissionManager = new PermissionManager(mockAssemblyService.Object, mockPermissionService.Object, mockRoleService.Object);

        // act
        var result = await permissionManager.RegisterAsync();

        // assert
        Assert.True(result);
        mockPermissionService.Verify(x => x.GetAllPermissionsAsync(), Times.Once);
        mockPermissionService.Verify(x => x.SavePermissionAsync(It.IsAny<Permission>()), Times.Never);
        mockPermissionService.Verify(x => x.DeletePermissionAsync(It.IsAny<int>()), Times.Never);
        mockRoleService.Verify(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>()), Times.Never);
    }

    [Fact]
    public async Task Register_WithNewActions_AddsPermissions() {
        // arrange
        var mockAssemblyService = new Mock<IAssemblyService>();
        mockAssemblyService.Setup(x => x.GetActionList()).Returns(_ActionList);

        var mockPermissionService = new Mock<IPermissionService>();
        mockPermissionService.Setup(x => x.GetAllPermissionsAsync()).ReturnsAsync(new List<Permission>());
        mockPermissionService.Setup(x => x.SavePermissionAsync(It.IsAny<Permission>())).ReturnsAsync(true);
        mockPermissionService.Setup(x => x.DeletePermissionAsync(It.IsAny<int>())).ReturnsAsync(true);

        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);

        var permissionManager = new PermissionManager(mockAssemblyService.Object, mockPermissionService.Object, mockRoleService.Object);

        // act
        var result = await permissionManager.RegisterAsync();

        // assert
        Assert.True(result);
        mockPermissionService.Verify(x => x.GetAllPermissionsAsync(), Times.Exactly(2));
        mockPermissionService.Verify(x => x.SavePermissionAsync(It.IsAny<Permission>()), Times.Exactly(3));
        mockPermissionService.Verify(x => x.DeletePermissionAsync(It.IsAny<int>()), Times.Never);
        mockRoleService.Verify(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>()), Times.Once);
    }

    [Fact]
    public async Task Register_WithDeletedActions_DeletesPermissions() {
        // arrange
        var mockAssemblyService = new Mock<IAssemblyService>();
        mockAssemblyService.Setup(x => x.GetActionList()).Returns([]);

        var mockPermissionService = new Mock<IPermissionService>();
        mockPermissionService.Setup(x => x.GetAllPermissionsAsync()).ReturnsAsync(_PermissionList);
        mockPermissionService.Setup(x => x.SavePermissionAsync(It.IsAny<Permission>())).ReturnsAsync(true);
        mockPermissionService.Setup(x => x.DeletePermissionAsync(It.IsAny<int>())).ReturnsAsync(true);

        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);

        var permissionManager = new PermissionManager(mockAssemblyService.Object, mockPermissionService.Object, mockRoleService.Object);

        // act
        var result = await permissionManager.RegisterAsync();

        // assert
        Assert.True(result);
        mockPermissionService.Verify(x => x.GetAllPermissionsAsync(), Times.Once);
        mockPermissionService.Verify(x => x.SavePermissionAsync(It.IsAny<Permission>()), Times.Never);
        mockPermissionService.Verify(x => x.DeletePermissionAsync(It.IsAny<int>()), Times.Exactly(3));
        mockRoleService.Verify(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>()), Times.Never);
    }

    [Fact]
    public async Task Register_WithServiceError_ReturnsFalse() {
        // arrange
        var mockAssemblyService = new Mock<IAssemblyService>();
        mockAssemblyService.Setup(x => x.GetActionList()).Returns([]);

        var mockPermissionService = new Mock<IPermissionService>();
        mockPermissionService.Setup(x => x.GetAllPermissionsAsync()).ReturnsAsync(_PermissionList);
        mockPermissionService.Setup(x => x.SavePermissionAsync(It.IsAny<Permission>())).ReturnsAsync(true);
        // returning false here will trigger the manager to return false
        mockPermissionService.Setup(x => x.DeletePermissionAsync(It.IsAny<int>())).ReturnsAsync(false);

        var mockRoleService = new Mock<IRoleService>();
        mockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);

        var permissionManager = new PermissionManager(mockAssemblyService.Object, mockPermissionService.Object, mockRoleService.Object);

        // act
        var result = await permissionManager.RegisterAsync();

        // assert
        Assert.False(result);
        mockPermissionService.Verify(x => x.GetAllPermissionsAsync(), Times.Once);
        mockPermissionService.Verify(x => x.SavePermissionAsync(It.IsAny<Permission>()), Times.Never);
        mockPermissionService.Verify(x => x.DeletePermissionAsync(It.IsAny<int>()), Times.Once);
        mockRoleService.Verify(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>()), Times.Never);
    }
}
