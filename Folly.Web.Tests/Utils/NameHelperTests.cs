using Folly.Utils;

namespace Folly.Web.Tests.Utils;

public class NameHelperTests {
    [Fact]
    public void DisplayName_WithFirstLast_ReturnsExpectedName() {
        // arrange
        var firstName = "first";
        var lastName = "last";

        // act
        var result = NameHelper.DisplayName(firstName, lastName);

        // assert
        Assert.Equal("last, first", result);
    }

    [Fact]
    public void DisplayName_WithWhitespace_ReturnsTrimmedName() {
        // arrange
        var firstName = "  first  ";
        var lastName = "  last  ";

        // act
        var result = NameHelper.DisplayName(firstName, lastName);

        // assert
        Assert.Equal("last, first", result);
    }

    [Fact]
    public void DisplayName_WithNoFirst_ReturnsLastName() {
        // arrange
        var lastName = "  last  ";

        // act
        var result = NameHelper.DisplayName(null, lastName);

        // assert
        Assert.Equal("last", result);
    }

    [Fact]
    public void DisplayName_WithNoLast_ReturnsFirstName() {
        // arrange
        var firstName = "  first  ";

        // act
        var result = NameHelper.DisplayName(firstName, null);

        // assert
        Assert.Equal("first", result);
    }

    [Fact]
    public void DisplayName_WithNoName_ReturnsEmptyString() {
        // arrange

        // act
        var result = NameHelper.DisplayName(null, null);

        // assert
        Assert.Equal("", result);
    }
}
