using Folly.Constants;
using Folly.Controllers;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;

namespace Folly.Web.Tests.Controllers;

public class RoleControllerTests() {
    private readonly Mock<IRoleService> _MockRoleService = new();
    private readonly Mock<IPermissionService> _MockPermissionService = new();
    private readonly Mock<IAssemblyService> _MockAssemblyService = new();
    private readonly Mock<ILogger<RoleController>> _MockLogger = new();
    private readonly Mock<IUrlHelper> _MockUrlHelper = new();

    private readonly string _Url = "/test";
    private readonly Role _RoleForSuccess = new() { Id = -100, Name = "success role" };
    private readonly Role _RoleForFailure = new() { Id = -101, Name = "failure role" };
    private readonly CopyRole _CopyRoleForSuccess = new() { Id = -100, Prompt = "success prompt" };
    private readonly CopyRole _CopyRoleForFailure = new() { Id = -101, Prompt = "failure prompt" };

    private RoleController CreateController() {
        _MockRoleService.Setup(x => x.GetRoleByIdAsync(_RoleForSuccess.Id)).ReturnsAsync(_RoleForSuccess);
        _MockRoleService.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(new List<Role> { _RoleForSuccess, _RoleForFailure });
        _MockRoleService.Setup(x => x.SaveRoleAsync(_RoleForSuccess)).ReturnsAsync(true);
        _MockRoleService.Setup(x => x.SaveRoleAsync(_RoleForFailure)).ReturnsAsync(false);
        _MockRoleService.Setup(x => x.CopyRoleAsync(_CopyRoleForSuccess)).ReturnsAsync(true);
        _MockRoleService.Setup(x => x.CopyRoleAsync(_CopyRoleForFailure)).ReturnsAsync(false);
        _MockRoleService.Setup(x => x.DeleteRoleAsync(_RoleForSuccess.Id)).ReturnsAsync(true);
        _MockRoleService.Setup(x => x.DeleteRoleAsync(_RoleForFailure.Id)).ReturnsAsync(false);
        _MockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);

        _MockAssemblyService.Setup(x => x.GetActionList()).Returns([]);

        _MockPermissionService.Setup(x => x.GetAllPermissionsAsync()).ReturnsAsync(new List<Permission>());
        _MockPermissionService.Setup(x => x.SavePermissionAsync(It.IsAny<Permission>())).ReturnsAsync(true);
        _MockPermissionService.Setup(x => x.DeletePermissionAsync(It.IsAny<int>())).ReturnsAsync(true);

        _MockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(_Url);

        return new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            },
            Url = _MockUrlHelper.Object
        };
    }

    [Fact]
    public void Get_Index_ReturnsView_WithNoModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
    }

    [Fact]
    public async Task Get_List_ReturnsData() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.List();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<RoleListResult>>(okResult.Value);
        var role = returnValue.FirstOrDefault();
        Assert.Collection(returnValue,
            x => Assert.Equal(_RoleForSuccess.Name, x.Name),
            x => Assert.Equal(_RoleForFailure.Name, x.Name)
        );
    }

    [Fact]
    public void Get_Create_ReturnsView_WithNewRoleModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Equal(0, model.Id);
    }

    [Fact]
    public async Task Post_Create_WithValidModel_ReturnsIndexView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Create(_RoleForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessSavingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(_Url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Post_Create_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Create(_RoleForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task Post_Create_WithServiceError_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Create(_RoleForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains(Roles.ErrorSavingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Get_Edit_WithValidId_ReturnsCreateEditViewWithModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_RoleForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Equal(_RoleForSuccess.Id, model.Id);
    }

    [Fact]
    public async Task Get_Edit_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(-200);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());
    }

    [Fact]
    public async Task Post_Edit_WithValidModel_ReturnsIndexView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_RoleForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessSavingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(_Url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Post_Edit_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Edit(_RoleForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task Post_Edit_WithServiceError_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_RoleForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains(Roles.ErrorSavingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Get_Copy_WithValidId_ReturnsCopyViewWithModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Copy(_RoleForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CopyRole>(viewResult.ViewData.Model);
        Assert.Equal("Copy", viewResult.ViewName);
        Assert.Equal(_RoleForSuccess.Id, model.Id);
    }
    [Fact]
    public async Task Get_Copy_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Copy(-200);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());
    }

    [Fact]
    public async Task Post_Copy_WithValidModel_ReturnsIndexView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Copy(_CopyRoleForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessCopyingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(_Url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.CopyRoleAsync(It.IsAny<CopyRole>()), Times.Once);
    }

    [Fact]
    public async Task Post_Copy_WithInvalidModel_ReturnsCopyViewWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Copy(_CopyRoleForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Copy", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.CopyRoleAsync(It.IsAny<CopyRole>()), Times.Never);
    }

    [Fact]
    public async Task Post_Copy_WithServiceError_ReturnsCopyViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Copy(_CopyRoleForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Copy", viewResult.ViewName);
        Assert.Contains(Roles.ErrorSavingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.CopyRoleAsync(It.IsAny<CopyRole>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Delete_WithValidId_ReturnsIndexViewWithMessage() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Delete(_RoleForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessDeletingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        _MockRoleService.Verify(x => x.DeleteRoleAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Delete_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Delete(_RoleForFailure.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.ErrorDeletingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        _MockRoleService.Verify(x => x.DeleteRoleAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Get_RefreshPermissions_ReturnsIndexViewWithMessage() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.RefreshPermissions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessRefreshingPermissions, viewResult.ViewData[ViewProperties.Message]?.ToString());
    }

    [Fact]
    public async Task Get_RefreshPermissions_WithServiceError_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();
        _MockRoleService.Setup(x => x.AddPermissionsToDefaultRoleAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(false);

        // Act
        var result = await controller.RefreshPermissions();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.ErrorRefreshingPermissions, viewResult.ViewData[ViewProperties.Error]?.ToString());
    }
}
