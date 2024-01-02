using Folly.Services;
using Folly.Web.Tests.Fixtures;
using DTO = Folly.Models;

namespace Folly.Web.Tests.Services;

[Collection(nameof(DatabaseCollection))]
public class LanguageServiceTests(DatabaseFixture fixture) {
    private readonly DatabaseFixture _Fixture = fixture;
    private readonly LanguageService _LanguageService = new(fixture.CreateContext());

    [Fact]
    public async Task GetLanguageByIdAsync_ReturnsEnglishLanguageDTO() {
        // arrange
        var englishLanguage = _Fixture.CreateContext().Languages.Find(1)!;

        // act
        var language = await _LanguageService.GetLanguageByIdAsync(1);

        // assert
        Assert.NotNull(language);
        Assert.IsType<DTO.Language>(language);
        Assert.Equal(englishLanguage.Name, language.Name);
        Assert.True(language.IsDefault);
        Assert.Equal(englishLanguage.LanguageCode, language.LanguageCode);
    }

    [Fact]
    public async Task GetLanguageByIdAsync_WithInvalidLanguageId_ThrowsException() {
        // arrange
        var languageIdToGet = -200;

        // act

        // assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _LanguageService.GetLanguageByIdAsync(languageIdToGet));
    }

    [Fact]
    public async Task GetAllLanguagesAsync_ReturnsTwoLanguageDTOs() {
        // arrange
        using var context = _Fixture.CreateContext();
        var englishLanguage = context.Languages.Find(1)!;
        var spanishLanguage = context.Languages.Find(2)!;

        // act
        var languages = await _LanguageService.GetAllLanguagesAsync();

        // assert
        Assert.NotEmpty(languages);
        Assert.IsAssignableFrom<IEnumerable<DTO.Language>>(languages);
        Assert.Equal(2, languages.Count());
        Assert.Collection(languages,
            x => Assert.Equal(englishLanguage.Id, x.Id),
            x => Assert.Equal(spanishLanguage.Id, x.Id)
        );
        Assert.Collection(languages,
            x => Assert.Equal(englishLanguage.Name, x.Name),
            x => Assert.Equal(spanishLanguage.Name, x.Name)
        );
    }
}
