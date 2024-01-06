using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Validators;
using Folly.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Folly.Web.Tests.Validators;

[Collection(nameof(DatabaseCollection))]
public class IsUniqueUserNameAttributeTests {
    private readonly DatabaseFixture _Fixture;
    private readonly UserService _UserService;
    private readonly Mock<IServiceProvider> _MockServiceProvider;

    public IsUniqueUserNameAttributeTests(DatabaseFixture fixture) {
        _Fixture = fixture;

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(x => x.HttpContext!.User.Identity).Returns(new GenericIdentity(fixture.TestUser.UserName, "test"));

        _UserService = new UserService(fixture.CreateContext(), mockHttpContextAccessor.Object);

        _MockServiceProvider = new Mock<IServiceProvider>();
        _MockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(_UserService);
    }

    public static TheoryData<User> ValidModels => new() {
        new User { Id = -1, UserName = "user" }, // matches DatabaseFixture.User.UserName
        new User { Id = -1, UserName = "test" },
        new User { Id = -100, UserName = "test" }
    };

    public static TheoryData<User> InvalidModels => new() {
        new User {Id = -100, UserName = "user"} // matches DatabaseFixture.User.UserName
    };

    [Theory]
    [MemberData(nameof(ValidModels))]
    public void Test_Validation_Passes(User model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsUniqueUserNameAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.Null(validationResult?.ErrorMessage);
    }

    [Theory]
    [MemberData(nameof(InvalidModels))]
    public void Test_Validation_Fails(User model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsUniqueUserNameAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.NotNull(validationResult?.ErrorMessage);
        Assert.Equal(Users.ErrorDuplicateUserName, validationResult?.ErrorMessage);
    }
}
