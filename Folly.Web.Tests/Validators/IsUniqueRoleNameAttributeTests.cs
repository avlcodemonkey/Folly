using System.ComponentModel.DataAnnotations;
using Folly.Models;
using Folly.Resources;
using Folly.Services;
using Folly.Validators;
using Folly.Web.Tests.Fixtures;
using Moq;

namespace Folly.Web.Tests.Validators;

[Collection(nameof(DatabaseCollection))]
public class IsUniqueRoleNameAttributeTests {
    private readonly DatabaseFixture _Fixture;
    private readonly RoleService _RoleService;
    private readonly Mock<IServiceProvider> _MockServiceProvider;

    public IsUniqueRoleNameAttributeTests(DatabaseFixture fixture) {
        _Fixture = fixture;

        _RoleService = new RoleService(fixture.CreateContext());

        _MockServiceProvider = new Mock<IServiceProvider>();
        _MockServiceProvider.Setup(x => x.GetService(typeof(IRoleService))).Returns(_RoleService);
    }

    public static TheoryData<Role> ValidModels => new() {
        new Role { Id = -1, Name = "Test" }, // matches DatabaseFixture.TestRole
        new Role { Id = -1, Name = "random name" },
        new Role { Id = -100, Name = "another random name" }
    };

    public static TheoryData<Role> InvalidModels => new() {
        new Role {Id = -100, Name = "Test"} // matches DatabaseFixture.TestRole.Name
    };

    [Theory]
    [MemberData(nameof(ValidModels))]
    public void Test_Validation_Passes(Role model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsUniqueRoleNameAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.Null(validationResult?.ErrorMessage);
    }

    [Theory]
    [MemberData(nameof(InvalidModels))]
    public void Test_Validation_Fails(Role model) {
        // arrange
        var validationContext = new ValidationContext(model, _MockServiceProvider.Object, null);

        // act
        var validationResult = new IsUniqueRoleNameAttribute().GetValidationResult(model, validationContext);

        // assert
        Assert.NotNull(validationResult?.ErrorMessage);
        Assert.Equal(Roles.ErrorDuplicateName, validationResult?.ErrorMessage);
    }
}
