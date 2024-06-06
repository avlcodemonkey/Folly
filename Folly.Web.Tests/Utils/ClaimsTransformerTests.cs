using System.Security.Claims;
using Folly.Models;
using Folly.Services;
using Folly.Utils;
using Moq;

namespace Folly.Web.Tests.Utils;

public class ClaimsTransformerTests {
    private readonly Mock<IUserService> _MockUserService;
    private readonly ClaimsTransformer _ClaimsTransformer;

    private readonly User _User = new() { Id = 1, UserName = "testUser" };

    private readonly List<UserClaim> _UserClaims = [
        new(1, 0, "controller1", "action1"),
        new(2, 0, "controller2", "action2"),
    ];

    public ClaimsTransformerTests() {
        _MockUserService = new Mock<IUserService>();
        _MockUserService.Setup(x => x.GetUserByUserNameAsync(_User.UserName)).ReturnsAsync(_User);
        _MockUserService.Setup(x => x.GetClaimsByUserIdAsync(_User.Id)).ReturnsAsync(_UserClaims);

        _ClaimsTransformer = new ClaimsTransformer(_MockUserService.Object);
    }

    [Fact]
    public async Task TransformAsync_WithNoIdentity_ReturnsEarly() {
        // arrange
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(x => x.Identity).Returns<ClaimsIdentity>(null!);

        // act
        var result = await _ClaimsTransformer.TransformAsync(mockClaimsPrincipal.Object);

        // assert
        Assert.IsAssignableFrom<ClaimsPrincipal>(result);
        mockClaimsPrincipal.Verify(x => x.Identity, Times.Once);
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task TransformAsync_WithNoName_ReturnsEarly() {
        // arrange
        var claimsIdentity = new ClaimsIdentity();
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(x => x.Identity).Returns(claimsIdentity);

        // act
        var result = await _ClaimsTransformer.TransformAsync(mockClaimsPrincipal.Object);

        // assert
        Assert.IsAssignableFrom<ClaimsPrincipal>(result);
        mockClaimsPrincipal.Verify(x => x.Identity, Times.Exactly(3));
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task TransformAsync_WithRoleClaim_ReturnsEarly() {
        // arrange
        var claimsIdentity = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, _User.UserName),
            new Claim(ClaimTypes.Role, "testRole")
        ]);

        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(x => x.Identity).Returns(claimsIdentity);

        // act
        var result = await _ClaimsTransformer.TransformAsync(mockClaimsPrincipal.Object);

        // assert
        Assert.IsAssignableFrom<ClaimsPrincipal>(result);
        mockClaimsPrincipal.Verify(x => x.Identity, Times.Exactly(2));
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task TransformAsync_WithNoMatchingUserName_ReturnsEarly() {
        // arrange
        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "nameThatDoesExist")]);

        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(x => x.Identity).Returns(claimsIdentity);

        // act
        var result = await _ClaimsTransformer.TransformAsync(mockClaimsPrincipal.Object);

        // assert
        Assert.IsAssignableFrom<ClaimsPrincipal>(result);
        mockClaimsPrincipal.Verify(x => x.Identity, Times.Exactly(4));
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        _MockUserService.Verify(x => x.GetClaimsByUserIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task TransformAsync_WithNoRoleClaims_AddsRoleClaims() {
        // arrange
        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.Name, _User.UserName)]);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // act
        var result = await _ClaimsTransformer.TransformAsync(claimsPrincipal);

        // assert
        Assert.IsAssignableFrom<ClaimsPrincipal>(result);
        Assert.Equal(3, claimsPrincipal.Claims.Count());
        Assert.Collection(claimsPrincipal.Claims,
            x => Assert.Equal(ClaimTypes.Name, x.Type),
            x => Assert.Equal(ClaimTypes.Role, x.Type),
            x => Assert.Equal(ClaimTypes.Role, x.Type)
        );
        Assert.Collection(claimsPrincipal.Claims,
            x => Assert.Equal(_User.UserName, x.Value),
            x => Assert.Equal("controller1.action1", x.Value),
            x => Assert.Equal("controller2.action2", x.Value)
        );
        _MockUserService.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        _MockUserService.Verify(x => x.GetClaimsByUserIdAsync(It.IsAny<int>()), Times.Once);
    }

}
