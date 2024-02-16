using Folly.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Folly.Web.Tests.Controllers;

/// <summary>
/// Error controller doesn't have much to it yet.  But it probably will, so stub this out.
/// </summary>
public class ErrorControllerTests() {
    private readonly Mock<ILogger<ErrorController>> _MockLogger = new();

    private ErrorController CreateController() {
        _MockLogger.Setup(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));

        return new ErrorController(_MockLogger.Object) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public void Get_Index_WithNoCode_ReturnsView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        _MockLogger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Never);
    }

    [Fact]
    public void Get_Index_WithCode_LogsErrorAndReturnsView() {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Index("code");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        _MockLogger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
    }
}
