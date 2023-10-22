using Folly.Domain.Models;

namespace Folly.Domain.Tests;

public class FollyDbContextTests : IClassFixture<TestDatabaseFixture> {
    private readonly TestDatabaseFixture Fixture;

    private readonly Language LanguageForCreate = new() { Id = -1, Name = "create language", CountryCode = "country1", LanguageCode = "lang1" };
    private readonly Language LanguageForUpdate = new() { Id = -2, Name = "update language", CountryCode = "country2", LanguageCode = "lang2" };

    public FollyDbContextTests(TestDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task SaveChangesAsync_SetsCreatedUser() {
        // arrange
        var createStartTime = DateTime.UtcNow;
        var createUserId = Fixture.UserForCreate.Id;

        // act
        Language createdLanguage;
        using (var dbContext = Fixture.CreateContextForCreate()) {
            dbContext.Languages.Add(LanguageForCreate);
            await dbContext.SaveChangesAsync();
            createdLanguage = dbContext.Languages.First(x => x.Id == LanguageForCreate.Id);
        }

        // assert
        Assert.NotNull(createdLanguage);
        Assert.Equal(createUserId, createdLanguage.CreatedUserId);
        Assert.True(createStartTime <= createdLanguage.CreatedDate);
        Assert.Equal(createUserId, createdLanguage.UpdatedUserId);
        Assert.True(createStartTime <= createdLanguage.UpdatedDate);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsUpdatedUser() {
        // arrange
        using (var dbContext = Fixture.CreateContextForCreate()) {
            dbContext.Languages.Add(LanguageForUpdate);
            await dbContext.SaveChangesAsync();
        }
        var createUserId = Fixture.UserForCreate.Id;
        var updateUserId = Fixture.UserForUpdate.Id;
        var updateStartTime = DateTime.UtcNow;
        var newLanguageName = "new language name";

        // act
        Language updatedLanguage;
        using (var dbContext = Fixture.CreateContextForUpdate()) {
            updatedLanguage = dbContext.Languages.First(x => x.Id == LanguageForUpdate.Id);
            updatedLanguage.Name = newLanguageName;
            dbContext.Languages.Update(updatedLanguage);
            await dbContext.SaveChangesAsync();
            updatedLanguage = dbContext.Languages.First(x => x.Id == LanguageForUpdate.Id);
        }

        // assert
        Assert.NotNull(updatedLanguage);
        Assert.Equal(newLanguageName, updatedLanguage.Name);
        Assert.Equal(createUserId, updatedLanguage.CreatedUserId);
        Assert.Equal(updateUserId, updatedLanguage.UpdatedUserId);
        Assert.True(updateStartTime >= updatedLanguage.CreatedDate);
        Assert.True(updateStartTime <= updatedLanguage.UpdatedDate);
    }
}
