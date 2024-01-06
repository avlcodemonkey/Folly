using System.ComponentModel.DataAnnotations;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Validators;
using Folly.Web.Tests.Fixtures;
using Moq;

namespace Folly.Web.Tests.Validators;

[Collection(nameof(DatabaseCollection))]
public class IsOnlyDefaultRoleAttributeTests {
    private readonly DatabaseFixture _Fixture;
    private readonly RoleService _RoleService;
    private readonly Mock<IServiceProvider> _MockServiceProvider;

    public IsOnlyDefaultRoleAttributeTests(DatabaseFixture fixture) {
        _Fixture = fixture;
        _RoleService = new RoleService(fixture.CreateContext());
        _MockServiceProvider = new Mock<IServiceProvider>();
        _MockServiceProvider.Setup(x => x.GetService(typeof(IRoleService))).Returns(_RoleService);
    }

    public static TheoryData<Role> ValidModels => new() {
        new Role { Id = 1, IsDefault = true }, // matches ModelBuilderExtensions Administrator roleId
        new Role { Id = 1, IsDefault = false },
        new Role { Id = -100, IsDefault = false }
    };

    public static TheoryData<Role> InvalidModels => new() {
        new Role {Id = -100, IsDefault = true }
    };

    [Theory]
    [MemberData(nameof(ValidModels))]
    public void Test_Validation_Passes(Role model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsOnlyDefaultRoleAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.Null(validationResult?.ErrorMessage);
    }

    [Theory]
    [MemberData(nameof(InvalidModels))]
    public void Test_Validation_Fails(Role model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsOnlyDefaultRoleAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.NotNull(validationResult?.ErrorMessage);
        Assert.Equal(Roles.ErrorDuplicateDefault, validationResult?.ErrorMessage);
    }
}
