using System.Security.Principal;
using Folly.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Folly.Domain.Tests;

public class FollyDbContextTests : IDisposable {
    private const string ConnectionString = "Filename=:memory:";
    private readonly SqliteConnection Connection;
    private readonly Mock<IConfiguration> MockConfiguration;

    private readonly User UserForCreate = new() { Id = -1, UserName = "create_user", Email = "create_user@fake.com", LanguageId = -1, FirstName = "Create" };
    private readonly User UserForUpdate = new() { Id = -2, UserName = "update_user", Email = "update_user@fake.com", LanguageId = -1, FirstName = "Update" };

    private readonly Language LanguageForCreate = new() { Id = -1, Name = "create language", CountryCode = "country1", LanguageCode = "lang1" };
    private readonly Language LanguageForUpdate = new() { Id = -2, Name = "update language", CountryCode = "country2", LanguageCode = "lang2" };

    public FollyDbContextTests() {
        MockConfiguration = new Mock<IConfiguration>();
        MockConfiguration.Setup(x => x.GetSection("App").GetSection("Database")["FilePath"]).Returns(ConnectionString);

        // Creates the SQLite in-memory database, which will persist until the connection is closed at the end of the test (see Dispose below).
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        // create the user data we will need for each test
        using var dbContext = CreateContext(UserForCreate);
        dbContext.Database.Migrate();
        dbContext.Users.Add(UserForCreate);
        dbContext.Users.Add(UserForUpdate);
        // SaveChanges instead of SaveChangesAsync to avoid running the SaveChangesAsync override we're testing here
        dbContext.SaveChanges();

    }

    public FollyDbContext CreateContext(User user)
        => new(new DbContextOptionsBuilder<FollyDbContext>().UseSqlite(Connection).Options, MockConfiguration.Object, CreateHttpContextAccessor(user));

    public static IHttpContextAccessor CreateHttpContextAccessor(User user) {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity).Returns(new GenericIdentity(user.UserName, "test"));
        return mockHttpContextAccessor.Object;
    }

    public void Dispose() {
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsCreatedUser() {
        // arrange
        var createStartTime = DateTime.UtcNow;
        using var dbContext = CreateContext(UserForCreate);

        // act
        dbContext.Languages.Add(LanguageForCreate);
        await dbContext.SaveChangesAsync();
        var savedLanguage = dbContext.Languages.First(x => x.Id == LanguageForCreate.Id);

        // assert
        Assert.Equal(UserForCreate.Id, savedLanguage.CreatedUserId);
        Assert.True(savedLanguage.CreatedDate >= createStartTime);
        Assert.Equal(UserForCreate.Id, savedLanguage.UpdatedUserId);
        Assert.True(savedLanguage.UpdatedDate >= createStartTime);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsUpdatedUser() {
        // arrange
        using (var dbContext = CreateContext(UserForCreate)) {
            dbContext.Languages.Add(LanguageForUpdate);
            await dbContext.SaveChangesAsync();
        }
        var updateStartTime = DateTime.UtcNow;

        // act
        Language updatedLanguage;
        using (var dbContext = CreateContext(UserForUpdate)) {
            updatedLanguage = dbContext.Languages.First(x => x.Id == LanguageForUpdate.Id);
            updatedLanguage.Name = "new language name";
            dbContext.Languages.Update(updatedLanguage);
            await dbContext.SaveChangesAsync();
            updatedLanguage = dbContext.Languages.First(x => x.Id == LanguageForUpdate.Id);
        }

        // assert
        Assert.NotNull(updatedLanguage);
        Assert.Equal(UserForCreate.Id, updatedLanguage.CreatedUserId);
        Assert.Equal(UserForUpdate.Id, updatedLanguage.UpdatedUserId);
        Assert.True(updatedLanguage.CreatedDate <= updateStartTime);
        Assert.True(updatedLanguage.UpdatedDate >= updateStartTime);
    }
}
