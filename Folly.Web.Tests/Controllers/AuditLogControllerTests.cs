using Folly.Constants;
using Folly.Controllers;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Folly.Web.Tests.Controllers;

public class AuditLogControllerTests() {
    private readonly Mock<IUserService> _MockUserService = new();
    private readonly Mock<IAuditLogService> _MockAuditLogService = new();
    private readonly Mock<ILogger<AuditLogController>> _MockLogger = new();

    private readonly AuditLog _AuditLogForSuccess = new() { Id = -100L, BatchId = Guid.NewGuid() };
    private readonly AuditLog _AuditLogForFailure = new() { Id = -101L, BatchId = Guid.NewGuid() };

    private AuditLogController CreateController() {
        _MockAuditLogService.Setup(x => x.GetLogByIdAsync(_AuditLogForSuccess.Id)).ReturnsAsync(_AuditLogForSuccess);
        _MockAuditLogService.Setup(x => x.GetLogByIdAsync(_AuditLogForFailure.Id)).ReturnsAsync(null as AuditLog);

        return new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);
    }

    [Fact]
    public void Get_Index_ReturnsView_WithSearchModel() {
        // Arrange
        var auditLogSearchModel = new AuditLogSearch { BatchId = Guid.NewGuid() };

        var controller = CreateController();

        // Act
        var result = controller.Index(auditLogSearchModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<AuditLogSearch>(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(auditLogSearchModel.BatchId, model.BatchId);
    }

    [Fact]
    public async Task Get_View_WithValidId_ReturnsView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.View(_AuditLogForSuccess.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<AuditLog>(viewResult.ViewData.Model);
        Assert.Equal("View", viewResult.ViewName);
        Assert.Equal(_AuditLogForSuccess.BatchId, model.BatchId);
    }

    [Fact]
    public async Task Get_View_WithInvalidId_ReturnsIndexWithError() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.View(_AuditLogForFailure.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        _ = Assert.IsAssignableFrom<AuditLogSearch>(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]);
    }

    [Fact]
    public async Task Get_UserList_WithNoQuery_ReturnsFullUserList() {
        // Arrange
        var user1 = new AutocompleteUser { Value = -1, Label = NameHelper.DisplayName("first", "last") };
        var user2 = new AutocompleteUser { Value = -2, Label = NameHelper.DisplayName("gib", "berish") };

        _MockUserService.Setup(x => x.FindAutocompleteUsersByNameAsync(It.IsAny<string>())).ReturnsAsync(new List<AutocompleteUser> { user1, user2 });
        var controller = CreateController();

        // Act
        var result = await controller.UserList("");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<AutocompleteUser>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
        Assert.Collection(returnValue,
            x => Assert.Equal(user1.Value, x.Value),
            x => Assert.Equal(user2.Value, x.Value)
        );
        Assert.Collection(returnValue,
            x => Assert.Equal(user1.Label, x.Label),
            x => Assert.Equal(user2.Label, x.Label)
        );
    }

    [Fact]
    public async Task Get_UserList_WithQuery_ReturnsMatchingUserList() {
        // Arrange
        var user = new AutocompleteUser { Value = -2, Label = NameHelper.DisplayName("gib", "berish") };

        _MockUserService.Setup(x => x.FindAutocompleteUsersByNameAsync(It.IsAny<string>())).ReturnsAsync(new List<AutocompleteUser> { user });
        var controller = CreateController();

        // Act
        var result = await controller.UserList("gib");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<AutocompleteUser>>(okResult.Value);
        var resultUser = Assert.Single(returnValue);
        Assert.Equal(user.Value, resultUser.Value);
        Assert.Equal(user.Label, resultUser.Label);
    }

    [Fact]
    public async Task Post_Search_WithMatches_ReturnsResultList() {
        // Arrange
        var auditLogSearchResult = new AuditLogSearchResult { Id = 1L, BatchId = Guid.NewGuid() };
        var auditLogSearchModel = new AuditLogSearch { BatchId = auditLogSearchResult.BatchId, StartDate = null, EndDate = null };

        _MockAuditLogService.Setup(x => x.SearchLogsAsync(It.IsAny<AuditLogSearch>())).ReturnsAsync(new List<AuditLogSearchResult> { auditLogSearchResult });
        var controller = CreateController();

        // Act
        var result = await controller.Search(auditLogSearchModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<AuditLogSearchResult>>(okResult.Value);
        var resultLog = Assert.Single(returnValue);
        Assert.Equal(auditLogSearchResult.Id, resultLog.Id);
    }

    [Fact]
    public async Task Post_Search_WithNoMatches_ReturnsEmptyResultList() {
        // Arrange
        var auditLogSearchModel = new AuditLogSearch { BatchId = Guid.NewGuid(), StartDate = null, EndDate = null };

        _MockAuditLogService.Setup(x => x.SearchLogsAsync(It.IsAny<AuditLogSearch>())).ReturnsAsync(new List<AuditLogSearchResult>());
        var controller = CreateController();

        // Act
        var result = await controller.Search(auditLogSearchModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<AuditLogSearchResult>>(okResult.Value);
        Assert.Empty(returnValue);
    }
}
