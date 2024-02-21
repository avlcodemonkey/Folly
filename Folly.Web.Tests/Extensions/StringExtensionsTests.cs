using Folly.Extensions;

namespace Folly.Web.Tests.Extensions;

public class StringExtensionsTests {
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("gibberish", false)]
    [InlineData("false", false)]
    [InlineData("False", false)]
    [InlineData("0", false)]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("1", true)]
    public void ToBool_ReturnsExpectedResult(string? val, bool expected) {
        // arrange

        // act
        var result = val.ToBool();

        // assert
        Assert.Equal(expected, result);
    }
}
