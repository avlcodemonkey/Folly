using System.Reflection;
using Folly.Utils;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Folly.Web.Tests.Utils;

public class VersionCheckMiddlewareTests {
    private readonly Mock<RequestDelegate> _MockRequestDelegate;
    private readonly VersionCheckMiddleware _VersionCheckMiddleware;
    private static string? _VersionNumber;

    public VersionCheckMiddlewareTests() {
        _VersionNumber = Assembly.GetAssembly(typeof(VersionCheckMiddleware))!.GetName().Version?.ToString() ?? "";

        _MockRequestDelegate = new Mock<RequestDelegate>();
        _VersionCheckMiddleware = new VersionCheckMiddleware(_MockRequestDelegate.Object);
    }

    [Fact]
    public void InvokeAsync_MatchingRequestVersionHeaders_AddsNoResponseHeader() {
        // arrange
        var mockHttpContext = new Mock<HttpContext>();
        var headerDictionary = new HeaderDictionary();
        mockHttpContext.Setup(x => x.Request.Headers[HtmxHeaders.Version]).Returns(_VersionNumber);
        mockHttpContext.Setup(x => x.Response.Headers).Returns(headerDictionary);

        // act
        var result = _VersionCheckMiddleware.InvokeAsync(mockHttpContext.Object);

        // assert
        Assert.False(headerDictionary.ContainsKey(HtmxHeaders.Refresh));
        mockHttpContext.Verify(x => x.Request.Headers, Times.Once);
        mockHttpContext.Verify(x => x.Response.Headers, Times.Never);
    }

    [Fact]
    public void InvokeAsync_MissingRequestVersionHeaders_AddsNoResponseHeader() {
        // arrange
        var mockHttpContext = new Mock<HttpContext>();
        var headerDictionary = new HeaderDictionary();
        mockHttpContext.Setup(x => x.Request.Headers).Returns(headerDictionary);
        mockHttpContext.Setup(x => x.Response.Headers).Returns(headerDictionary);

        // act
        var result = _VersionCheckMiddleware.InvokeAsync(mockHttpContext.Object);

        // assert
        Assert.False(headerDictionary.ContainsKey(HtmxHeaders.Refresh));
        mockHttpContext.Verify(x => x.Request.Headers, Times.Once);
        mockHttpContext.Verify(x => x.Response.Headers, Times.Never);
    }

    [Fact]
    public void InvokeAsync_MismatchRequestVersionHeaders_AddsResponseHeader() {
        // arrange
        var mockHttpContext = new Mock<HttpContext>();
        var headerDictionary = new HeaderDictionary();
        mockHttpContext.Setup(x => x.Request.Headers[HtmxHeaders.Version]).Returns("-1.0.0.0");
        mockHttpContext.Setup(x => x.Response.Headers).Returns(headerDictionary);

        // act
        var result = _VersionCheckMiddleware.InvokeAsync(mockHttpContext.Object);

        // assert
        Assert.True(headerDictionary.ContainsKey(HtmxHeaders.Refresh));
        mockHttpContext.Verify(x => x.Request.Headers, Times.Once);
        mockHttpContext.Verify(x => x.Response.Headers, Times.Once);
    }
}
