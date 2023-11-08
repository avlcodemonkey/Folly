using Folly.Domain.Models;

namespace Folly.Domain.Tests;

public class FollyDbContextTests : IClassFixture<TestDatabaseFixture> {
    private readonly TestDatabaseFixture _Fixture;

    private readonly Language _LanguageForCreate = new() { Id = -1, Name = "create language", CountryCode = "country1", LanguageCode = "lang1" };
    private readonly Language _LanguageForUpdate = new() { Id = -2, Name = "update language", CountryCode = "country2", LanguageCode = "lang2" };

    public FollyDbContextTests(TestDatabaseFixture fixture) => _Fixture = fixture;

    [Fact]
    public async Task SaveChangesAsync_SetsCreatedUser() {
        // arrange
        var createStartTime = DateTime.UtcNow;
        var createUserId = _Fixture.UserForCreate.Id;

        // act
        Language createdLanguage;
        using (var dbContext = _Fixture.CreateContextForCreate()) {
            dbContext.Languages.Add(_LanguageForCreate);
            await dbContext.SaveChangesAsync();
            createdLanguage = dbContext.Languages.First(x => x.Id == _LanguageForCreate.Id);
        }

        // assert
        Assert.NotNull(createdLanguage);
        Assert.True(createStartTime <= createdLanguage.CreatedDate);
        Assert.True(createStartTime <= createdLanguage.UpdatedDate);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsUpdatedUser() {
        // arrange
        using (var dbContext = _Fixture.CreateContextForCreate()) {
            dbContext.Languages.Add(_LanguageForUpdate);
            await dbContext.SaveChangesAsync();
        }
        var createUserId = _Fixture.UserForCreate.Id;
        var updateUserId = _Fixture.UserForUpdate.Id;
        var updateStartTime = DateTime.UtcNow;
        var newLanguageName = "new language name";

        // act
        Language updatedLanguage;
        using (var dbContext = _Fixture.CreateContextForUpdate()) {
            updatedLanguage = dbContext.Languages.First(x => x.Id == _LanguageForUpdate.Id);
            updatedLanguage.Name = newLanguageName;
            dbContext.Languages.Update(updatedLanguage);
            await dbContext.SaveChangesAsync();
            updatedLanguage = dbContext.Languages.First(x => x.Id == _LanguageForUpdate.Id);
        }

        // assert
        Assert.NotNull(updatedLanguage);
        Assert.Equal(newLanguageName, updatedLanguage.Name);
        Assert.True(updateStartTime >= updatedLanguage.CreatedDate);
        Assert.True(updateStartTime <= updatedLanguage.UpdatedDate);
    }
}
