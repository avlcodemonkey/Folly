using Folly.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace Folly.Web.Tests.Extensions;

public class UrlHelperExtensionsTests {
    private readonly Mock<IUrlHelper> _MockUrlHelper = new();

    [Fact]
    public void ActionForMustache_ReturnsTemplatedUrl() {
        // arrange
        _MockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("/test");

        // act
        var url = _MockUrlHelper.Object.ActionForMustache("test");

        // assert
        Assert.Equal("/test/{{id}}", url);
    }
}
