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

public class UserControllerTests() {
    private readonly Mock<IUserService> _MockUserService = new();
    private readonly Mock<ILogger<UserController>> _MockLogger = new();
    private readonly Mock<IUrlHelper> _MockUrlHelper = new();

    private readonly string _Url = "/test";
    private readonly User _UserForSuccess = new() { Id = -100, UserName = "successUser", FirstName = "first1", LastName = "last1", Email = "a@b.com" };
    private readonly User _UserForFailure = new() { Id = -101, UserName = "failureUser", FirstName = "first2", LastName = null, Email = "b@c.com" };

    private UserController CreateController() {
        _MockUserService.Setup(x => x.GetUserByIdAsync(_UserForSuccess.Id)).ReturnsAsync(_UserForSuccess);
        _MockUserService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(new List<User> { _UserForSuccess, _UserForFailure });
        _MockUserService.Setup(x => x.SaveUserAsync(_UserForSuccess)).ReturnsAsync(true);
        _MockUserService.Setup(x => x.SaveUserAsync(_UserForFailure)).ReturnsAsync(false);
        _MockUserService.Setup(x => x.DeleteUserAsync(_UserForSuccess.Id)).ReturnsAsync(true);
        _MockUserService.Setup(x => x.DeleteUserAsync(_UserForFailure.Id)).ReturnsAsync(false);

        _MockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(_Url);

        return new UserController(_MockUserService.Object, _MockLogger.Object) {
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
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task Get_List_ReturnsData() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.List();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<UserListResult>>(okResult.Value);
        var role = returnValue.FirstOrDefault();
        Assert.Collection(returnValue,
            x => Assert.Equal(_UserForSuccess.Id, x.Id),
            x => Assert.Equal(_UserForFailure.Id, x.Id)
        );
        Assert.Collection(returnValue,
            x => Assert.Equal(_UserForSuccess.UserName, x.UserName),
            x => Assert.Equal(_UserForFailure.UserName, x.UserName)
        );
        Assert.Collection(returnValue,
            x => Assert.Equal(_UserForSuccess.FirstName, x.FirstName),
            x => Assert.Equal(_UserForFailure.FirstName, x.FirstName)
        );
        Assert.Collection(returnValue,
            x => Assert.Equal(_UserForSuccess.LastName, x.LastName),
            x => Assert.Equal("", x.LastName)
        );
        Assert.Collection(returnValue,
            x => Assert.Equal(_UserForSuccess.Email, x.Email),
            x => Assert.Equal(_UserForFailure.Email, x.Email)
        );
    }

    [Fact]
    public void Get_Create_ReturnsView_WithNewUserModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(0, model.Id);
    }

    [Fact]
    public async Task Post_Create_WithValidModel_ReturnsIndexView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Create(_UserForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Users.SuccessSavingUser, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.Null(viewResult.ViewData.Model);

        Assert.True(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));
        Assert.Equal(_Url, headerUrl.ToString());

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Post_Create_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Create(_UserForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());

        Assert.False(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(_UserForSuccess.Id, model.Id);

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Post_Create_WithServiceError_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Create(_UserForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains(Users.ErrorSavingUser, viewResult.ViewData[ViewProperties.Error]?.ToString());

        Assert.False(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(_UserForFailure.Id, model.Id);

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Get_Edit_WithValidId_ReturnsCreateEditViewWithModel() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_UserForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(_UserForSuccess.Id, model.Id);
    }

    [Fact]
    public async Task Get_Edit_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(-200);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task Post_Edit_WithValidModel_ReturnsIndexView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_UserForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Users.SuccessSavingUser, viewResult.ViewData[ViewProperties.Message]?.ToString());

        Assert.True(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));
        Assert.Equal(_Url, headerUrl.ToString());

        Assert.Null(viewResult.ViewData.Model);

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Post_Edit_WithInvalidModel_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.Edit(_UserForSuccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());

        Assert.False(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(_UserForSuccess.Id, model.Id);

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Post_Edit_WithServiceError_ReturnsCreateEditViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Edit(_UserForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateEdit", viewResult.ViewName);
        Assert.Contains(Users.ErrorSavingUser, viewResult.ViewData[ViewProperties.Error]?.ToString());

        Assert.False(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));

        var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
        Assert.Equal(_UserForFailure.Id, model.Id);

        _MockUserService.Verify(x => x.SaveUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Delete_WithValidId_ReturnsIndexViewWithMessage() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Delete(_UserForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Users.SuccessDeletingUser, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.Null(viewResult.ViewData.Model);

        _MockUserService.Verify(x => x.DeleteUserAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Delete_WithInvalidId_ReturnsIndexViewWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Delete(_UserForFailure.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Users.ErrorDeletingUser, viewResult.ViewData[ViewProperties.Error]?.ToString());
        Assert.Null(viewResult.ViewData.Model);

        _MockUserService.Verify(x => x.DeleteUserAsync(It.IsAny<int>()), Times.Once);
    }
}
