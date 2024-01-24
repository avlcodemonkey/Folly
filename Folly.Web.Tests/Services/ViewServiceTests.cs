using Folly.Models;
using Folly.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;

namespace Folly.Web.Tests.Services;

public class ViewServiceTests {
    private readonly ViewService _ViewService;

    private readonly Language _Language1 = new() { Id = 1, Name = "English", LanguageCode = "en" };
    private readonly Language _Language2 = new() { Id = 2, Name = "Spanish", LanguageCode = "es" };

    private readonly Permission _Permission1 = new() { Id = 1, ControllerName = "controller1", ActionName = "action1" };
    private readonly Permission _Permission2 = new() { Id = 2, ControllerName = "controller2", ActionName = "action2" };
    private readonly Permission _Permission3 = new() { Id = 3, ControllerName = "controller2", ActionName = "action3" };

    public ViewServiceTests() {
        var languageService = new Mock<ILanguageService>();
        languageService.Setup(x => x.GetAllLanguagesAsync().Result).Returns(new List<Language> { _Language1, _Language2 }.AsEnumerable());

        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(x => x.GetAllPermissionsAsync().Result).Returns(() => new List<Permission> { _Permission1, _Permission2, _Permission3 });

        _ViewService = new ViewService(languageService.Object, permissionService.Object, new Mock<IRoleService>().Object, new Mock<IUserService>().Object);
    }

    [Fact]
    public async Task GetLanguageSelectListAsync_ReturnsTwoSelectListItems() {
        // arrange

        // act
        var languages = await _ViewService.GetLanguageSelectListAsync();

        // assert
        Assert.NotEmpty(languages);
        Assert.IsAssignableFrom<IEnumerable<SelectListItem>>(languages);
        Assert.Equal(2, languages.Count());
        Assert.Collection(languages,
            x => Assert.Equal(_Language1.Id.ToString(), x.Value),
            x => Assert.Equal(_Language2.Id.ToString(), x.Value)
        );
        Assert.Collection(languages,
            x => Assert.Equal(_Language1.Name, x.Text),
            x => Assert.Equal(_Language2.Name, x.Text)
        );
    }

    [Fact]
    public async Task GetControllerPermissionsAsync_ReturnsDictionaryWithPermissions() {
        // arrange

        // act
        var permissions = await _ViewService.GetControllerPermissionsAsync();

        // assert
        Assert.NotEmpty(permissions);
        Assert.IsAssignableFrom<Dictionary<string, List<Permission>>>(permissions);

        Assert.Contains(_Permission1.ControllerName, permissions.Keys);
        Assert.Contains(_Permission2.ControllerName, permissions.Keys);
        Assert.Equal(2, permissions.Count);
        Assert.Contains(_Permission1, permissions[_Permission1.ControllerName]);
        Assert.Equal(2, permissions[_Permission2.ControllerName].Count);
        Assert.Contains(_Permission2, permissions[_Permission2.ControllerName]);
        Assert.Contains(_Permission3, permissions[_Permission2.ControllerName]);
    }
}
