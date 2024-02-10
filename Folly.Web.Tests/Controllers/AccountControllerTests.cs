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

    [Fact]
    public void Get_ToggleContextHelp_EnablesHelp() {
        // Arrange
        var mockSession = new Mock<ISession>();
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext() {
                    Session = mockSession.Object
                }
            }
        };

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
        var user = new User { UserName = "test", Email = "a@b.com" };
        _MockUserService.Setup(x => x.GetUserByUserNameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.Name, user.UserName)]);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext() {
                    User = claimsPrincipal
                }
            }
        };

        // Act
        var result = await controller.UpdateAccount();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(user.Email, model.Email);
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Post_UpdateAccount_WithInvalidModel_ReturnsUpdateWithError() {
        // Arrange
        var updateAccount = new UpdateAccount { FirstName = "first", LastName = "last", Email = "a@b.com" };
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object);
        controller.ModelState.AddModelError("error", "some error");

        // Act
        var result = await controller.UpdateAccount(updateAccount);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(updateAccount.Email, model.Email);
        Assert.Contains("some error", viewResult.ViewData[ViewProperties.Error]?.ToString());
    }

    [Fact]
    public async Task Post_UpdateAccount_WithValidModel_UpdatesAccount() {
        // Arrange
        var updateAccount = new UpdateAccount { FirstName = "first", LastName = "last", Email = "a@b.com" };
        var language = new Language { Id = 1, Name = "test lang", LanguageCode = "en" };
        var user = new User { UserName = "test", Email = "a@b.com" };
        _MockUserService.Setup(x => x.UpdateAccountAsync(It.IsAny<UpdateAccount>())).ReturnsAsync("");
        _MockLanguageService.Setup(x => x.GetLanguageByIdAsync(It.IsAny<int>())).ReturnsAsync(language);
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.UpdateAccount(updateAccount);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(updateAccount.Email, model.Email);
        Assert.Equal(Account.AccountUpdated, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.True(controller.Response.Headers.TryGetValue("Set-Cookie", out var cookieValue));
        Assert.Contains(CookieRequestCultureProvider.DefaultCookieName, cookieValue.ToString());
    }

    [Fact]
    public async Task Post_UpdateAccount_WithUserServiceError_ReturnsError() {
        // Arrange
        var updateAccount = new UpdateAccount { FirstName = "first", LastName = "last", Email = "a@b.com" };
        var language = new Language { Id = 1, Name = "test lang", LanguageCode = "en" };
        var user = new User { UserName = "test", Email = "a@b.com" };
        _MockUserService.Setup(x => x.UpdateAccountAsync(It.IsAny<UpdateAccount>())).ReturnsAsync("gibberish");
        _MockLanguageService.Setup(x => x.GetLanguageByIdAsync(It.IsAny<int>())).ReturnsAsync(language);
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.UpdateAccount(updateAccount);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateAccount", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<UpdateAccount>(viewResult.ViewData.Model);
        Assert.Equal(updateAccount.Email, model.Email);
        Assert.Equal("gibberish", viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.False(controller.Response.Headers.TryGetValue("Set-Cookie", out var cookieValue));
    }

    [Fact]
    public void Get_AccessDenied_ReturnsView() {
        // Arrange
        var controller = new AccountController(_MockUserService.Object, _MockLanguageService.Object, _MockLogger.Object);

        // Act
        var result = controller.AccessDenied();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        Assert.Equal(Core.ErrorGeneric, viewResult.ViewData[ViewProperties.Message]?.ToString());
        Assert.Null(viewResult.ViewData.Model);
    }
}
