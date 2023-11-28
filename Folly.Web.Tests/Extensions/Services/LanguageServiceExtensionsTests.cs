using Folly.Domain.Models;
using Folly.Extensions.Services;

namespace Folly.Web.Tests.Extensions.Services;

public class LanguageServiceExtensionsTests {

    public LanguageServiceExtensionsTests() { }

    [Fact]
    public void SelectSingleAsDTO_ReturnsProjectedDTO() {
        // arrange
        var language = new Language { Id = 1, Name = "test language", IsDefault = true, CountryCode = "country code", LanguageCode = "language code" };
        var languages = new List<Language> { language }.AsQueryable();

        // act
        var dtos = languages.SelectAsDTO();

        // assert
        Assert.NotNull(dtos);
        Assert.Single(dtos);

        Assert.All(dtos, x => Assert.Equal(language.Id, x.Id));
        Assert.All(dtos, x => Assert.Equal(language.Name, x.Name));
        Assert.All(dtos, x => Assert.Equal(language.IsDefault, x.IsDefault));
        Assert.All(dtos, x => Assert.Equal(language.LanguageCode, x.LanguageCode));
    }

    [Fact]
    public void SelectMultipleAsDTO_ReturnsProjectedDTOs() {
        // arrange
        var language1 = new Language { Id = 1, Name = "test language 1", IsDefault = true, CountryCode = "country code 1", LanguageCode = "language code 1" };
        var language2 = new Language { Id = 2, Name = "test language 2", IsDefault = false, CountryCode = "country code 2", LanguageCode = "language code 2" };
        var languages = new List<Language> { language1, language2 }.AsQueryable();

        // act
        var dtos = languages.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(language1.Id, x.Id),
            x => Assert.Equal(language2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(language1.Name, x.Name),
            x => Assert.Equal(language2.Name, x.Name)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(language1.IsDefault, x.IsDefault),
            x => Assert.Equal(language2.IsDefault, x.IsDefault)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(language1.LanguageCode, x.LanguageCode),
            x => Assert.Equal(language2.LanguageCode, x.LanguageCode)
        );
    }
}
