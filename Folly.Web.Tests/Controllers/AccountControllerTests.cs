using System.Security.Claims;
using Folly.Constants;
using Folly.Controllers;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Folly.Web.Tests.Controllers;

public class AccountControllerTests() {
    private readonly Mock<IUserService> _MockUserService = new();
    private readonly Mock<ILanguageService> _MockLanguageService = new();
    private readonly Mock<ILogger<AccountController>> _MockLogger = new();

    private readonly User _UserForSuccess = new() { UserName = "success", Email = "a@b.com" };
    private readonly User _UserForFailure = new() { UserName = "failure", Email = "b@c.com" };
    private readonly UpdateAccount _UpdateAccountForSucccess = new() { FirstName = "success", LastName = "last", Email = "a@b.com" };
    private readonly UpdateAccount _UpdateAccountForFailure = new() { FirstName = "failure", LastName = "last", Email = "b@c.com" };
    private readonly Language _Language = new() { Id = 1, Name = "test lang", LanguageCode = "en" };

    private AccountController CreateController(string? userName = null) {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, userName ?? _UserForSuccess.UserName)]));

        _MockUserService.Setup(x => x.GetUserByUserNameAsync(_UserForSuccess.UserName)).ReturnsAsync(_UserForSuccess);
        _MockUserService.Setup(x => x.GetUserByUserNameAsync(_UserForFailure.UserName)).ReturnsAsync(null as User);
        _MockUserService.Setup(x => x.UpdateAccountAsync(_UpdateAccountForSucccess)).ReturnsAsync("");
        _MockUserService.Setup(x => x.UpdateAccountAsync(_UpdateAccountForFailure)).ReturnsAsync("gibberish");

        _MockLanguageService.Setup(x => x.GetLanguageByIdAsync(It.IsAny<int>())).ReturnsAsync(_Language);

        return new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext() {
                    Session = new Mock<ISession>().Object,
                    User = claimsPrincipal
                }
            }
        };
    }

    [Fact]
    public void Get_ToggleContextHelp_EnablesHelp() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.ToggleContextHelp();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("ToggleContextHelp", viewResult.ViewName);
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task Get_UpdateAccount_WithValidUser_ReturnsView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.UpdateAccount();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);

        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(_UserForSuccess.Email, model.Email);

        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Get_UpdateAccount_WithInvalidUser_ReturnsError() {
        // Arrange
        var controller = CreateController(_UserForFailure.UserName);

        // Act
        var result = await controller.UpdateAccount();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]?.ToString());

        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Post_UpdateAccount_WithInvalidModel_ReturnsUpdateWithError() {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.UpdateAccount(_UpdateAccountForSucccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());

        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(_UpdateAccountForSucccess.Email, model.Email);

        _MockUserService.Verify(x => x.UpdateAccountAsync(It.IsAny<UpdateAccount>()), Times.Never);
    }

    [Fact]
    public async Task Post_UpdateAccount_WithValidModel_UpdatesAccount() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.UpdateAccount(_UpdateAccountForSucccess);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        Assert.Equal(Account.AccountUpdated, viewResult.ViewData[ViewProperties.Message]?.ToString());

        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(_UpdateAccountForSucccess.Email, model.Email);

        Assert.True(controller.Response.Headers.TryGetValue("Set-Cookie", out var cookieValue));
        Assert.Contains(CookieRequestCultureProvider.DefaultCookieName, cookieValue.ToString());

        _MockUserService.Verify(x => x.UpdateAccountAsync(It.IsAny<UpdateAccount>()), Times.Once);
    }

    [Fact]
    public async Task Post_UpdateAccount_WithUserServiceError_ReturnsError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.UpdateAccount(_UpdateAccountForFailure);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        Assert.Equal("gibberish", viewResult.ViewData[ViewProperties.Message]?.ToString());

        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(_UpdateAccountForFailure.Email, model.Email);

        Assert.False(controller.Response.Headers.TryGetValue("Set-Cookie", out _));

        _MockUserService.Verify(x => x.UpdateAccountAsync(It.IsAny<UpdateAccount>()), Times.Once);
    }

    [Fact]
    public void Get_AccessDenied_ReturnsView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.AccessDenied();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        Assert.Equal(Core.ErrorGeneric, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public void Get_PushUrl_WithNotEmptyUrl_AddsHeader() {
        // Arrange
        // PushUrl is part of the abstract BaseController, so use an AccountController instance to test it.
        var url = "/test";

        var controller = CreateController();

        // Act
        controller.PushUrl(url);

        // Assert
        Assert.True(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var headerUrl));
        Assert.Equal(url, headerUrl.ToString());
    }

    [Fact]
    public void Get_PushUrl_WithEmptyUrl_DoesNotAddHeader() {
        // Arrange
        // PushUrl is part of the abstract BaseController, so use an AccountController instance to test it.
        var controller = CreateController();

        // Act
        controller.PushUrl("");

        // Assert
        Assert.False(controller.Response.Headers.TryGetValue(PJax.PushUrl, out var _));
    }
}
