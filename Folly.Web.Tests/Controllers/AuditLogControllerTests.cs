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

    [Fact]
    public void Get_Index_ReturnsView_WithSearchModel() {
        // Arrange
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);
        var auditLogSearchModel = new AuditLogSearch() { BatchId = Guid.NewGuid() };

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
        var auditLog = new AuditLog { Id = 1L, BatchId = Guid.NewGuid() };
        _MockAuditLogService.Setup(x => x.GetLogByIdAsync(It.IsAny<long>())).ReturnsAsync(auditLog);
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);

        // Act
        var result = await controller.View(1L);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<AuditLog>(viewResult.ViewData.Model);
        Assert.Equal("View", viewResult.ViewName);
        Assert.Equal(auditLog.BatchId, model.BatchId);
    }

    [Fact]
    public async Task Get_View_WithInvalidId_ReturnsIndexWithError() {
        // Arrange
        _MockAuditLogService.Setup(x => x.GetLogByIdAsync(It.IsAny<long>())).ReturnsAsync(null as AuditLog);
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);

        // Act
        var result = await controller.View(1L);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<AuditLogSearch>(viewResult.ViewData.Model);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(Core.ErrorInvalidId, viewResult.ViewData[ViewProperties.Error]);
    }

    [Fact]
    public async Task Get_UserList_WithNoQuery_ReturnsFullUserList() {
        // Arrange
        var user1 = new AutocompleteUser { Value = -1, Label = "last, first" };
        var user2 = new AutocompleteUser { Value = -2, Label = "berish, gib" };
        _MockUserService.Setup(x => x.FindAutocompleteUsersByNameAsync(It.IsAny<string>())).ReturnsAsync(new List<AutocompleteUser> { user1, user2 });
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);

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
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);

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
        _MockAuditLogService.Setup(x => x.SearchLogsAsync(It.IsAny<AuditLogSearch>())).ReturnsAsync(new List<AuditLogSearchResult> { auditLogSearchResult });
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);
        var auditLogSearchModel = new AuditLogSearch() { BatchId = auditLogSearchResult.BatchId, StartDate = null, EndDate = null };

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
        _MockAuditLogService.Setup(x => x.SearchLogsAsync(It.IsAny<AuditLogSearch>())).ReturnsAsync(new List<AuditLogSearchResult>());
        var controller = new AuditLogController(_MockAuditLogService.Object, _MockUserService.Object, _MockLogger.Object);
        var auditLogSearchModel = new AuditLogSearch() { BatchId = Guid.NewGuid(), StartDate = null, EndDate = null };

        // Act
        var result = await controller.Search(auditLogSearchModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<AuditLogSearchResult>>(okResult.Value);
        Assert.Empty(returnValue);
    }
}
