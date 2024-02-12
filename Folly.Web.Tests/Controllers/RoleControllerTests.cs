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

    [Fact]
    public void Get_Index_ReturnsView_WithNoModel() {
        // Arrange
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
    }

    [Fact]
    public void Get_Create_ReturnsView_WithNewRoleModel() {
        // Arrange
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object);

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
        var role = new Role { Id = -100, Name = "test" };
        var url = "/test";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(url);
        _MockRoleService.Setup(x => x.SaveRoleAsync(It.IsAny<Role>())).ReturnsAsync(true);

        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            },
            Url = mockUrlHelper.Object
        };

        // Act
        var result = await controller.Create(role);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessSavingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Post_Create_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var role = new Role { Id = -100, Name = "test" };
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Create(role);

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
        var role = new Role { Id = -100, Name = "test" };
        _MockRoleService.Setup(x => x.SaveRoleAsync(It.IsAny<Role>())).ReturnsAsync(false);
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.Create(role);

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
        var role = new Role { Id = -100, Name = "test" };
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object);
        _MockRoleService.Setup(x => x.GetRoleByIdAsync(It.IsAny<int>())).ReturnsAsync(role);

        // Act
        var result = await controller.Edit(-100);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Equal(role.Id, model.Id);
    }

    [Fact]
    public async Task Get_Edit_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var role = new Role { Id = -100, Name = "test" };
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object);
        _MockRoleService.Setup(x => x.GetRoleByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Role);

        // Act
        var result = await controller.Edit(-100);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());
    }

    [Fact]
    public async Task Post_Edit_WithValidModel_ReturnsIndexView() {
        // Arrange
        var role = new Role { Id = -100, Name = "test" };
        var url = "/test";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(url);
        _MockRoleService.Setup(x => x.GetRoleByIdAsync(It.IsAny<int>())).ReturnsAsync(role);
        _MockRoleService.Setup(x => x.SaveRoleAsync(It.IsAny<Role>())).ReturnsAsync(true);

        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            },
            Url = mockUrlHelper.Object
        };

        // Act
        var result = await controller.Edit(role);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessSavingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Post_Edit_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var role = new Role { Id = -100, Name = "test" };
        _MockRoleService.Setup(x => x.SaveRoleAsync(It.IsAny<Role>())).ReturnsAsync(true);
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Edit(role);

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
        var role = new Role { Id = -100, Name = "test" };
        _MockRoleService.Setup(x => x.SaveRoleAsync(It.IsAny<Role>())).ReturnsAsync(false);
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.Edit(role);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains(Roles.ErrorSavingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.SaveRoleAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task Get_Copy_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var role = new Role { Id = -100, Name = "test" };
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object);
        _MockRoleService.Setup(x => x.GetRoleByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Role);

        // Act
        var result = await controller.Copy(role.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());
    }

    [Fact]
    public async Task Post_Copy_WithValidModel_ReturnsIndexView() {
        // Arrange
        var copyRole = new CopyRole { Id = -100, Prompt = "prompt" };
        var url = "/test";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(url);
        _MockRoleService.Setup(x => x.CopyRoleAsync(It.IsAny<CopyRole>())).ReturnsAsync(true);

        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            },
            Url = mockUrlHelper.Object
        };

        // Act
        var result = await controller.Copy(copyRole);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Roles.SuccessCopyingRole, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        Assert.Equal(url, headerUrl.ToString());
        _MockRoleService.Verify(x => x.CopyRoleAsync(It.IsAny<CopyRole>()), Times.Once);
    }

    [Fact]
    public async Task Post_Copy_WithInvalidModel_ReturnsCopyViewWithError() {
        // Arrange
        var copyRole = new CopyRole { Id = -100, Prompt = "prompt" };
        _MockRoleService.Setup(x => x.CopyRoleAsync(It.IsAny<CopyRole>())).ReturnsAsync(false);
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Copy(copyRole);

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
        var copyRole = new CopyRole { Id = -100, Prompt = "prompt" };
        var url = "/test";
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(url);
        _MockRoleService.Setup(x => x.CopyRoleAsync(It.IsAny<CopyRole>())).ReturnsAsync(false);
        var controller = new RoleController(_MockRoleService.Object, _MockPermissionService.Object, _MockAssemblyService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            },
            Url = mockUrlHelper.Object
        };

        // Act
        var result = await controller.Copy(copyRole);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Copy", viewResult.ViewName);
        Assert.Contains(Roles.ErrorSavingRole, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue(HtmxHeaders.PushUrl, out var headerUrl));
        _MockRoleService.Verify(x => x.CopyRoleAsync(It.IsAny<CopyRole>()), Times.Once);
    }
}
