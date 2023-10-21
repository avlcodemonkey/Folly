using System.Security.Principal;
using Folly.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Folly.Domain.Tests;

public class FollyDbContextTests {
    // @todo this works for now, but need to find a different way
    private const string ConnectionString = "Data Source = ..\\..\\..\\..\\AppData\\Folly.db;";

    private Mock<IHttpContextAccessor> MockHttpContextAccessor { get; set; }

    private readonly User TestUser = new() { Id = -1, UserName = "test_user", Email = "email@fake.com", LanguageId = -1, FirstName = "test" };

    public FollyDbContextTests() {
        MockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var identity = new GenericIdentity(TestUser.UserName, "test");
        MockHttpContextAccessor.Setup(req => req.HttpContext.User.Identity).Returns(identity);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsCreatedUser() {
        var options = new DbContextOptionsBuilder<FollyDbContext>()
            .UseSqlite(ConnectionString)
            .Options;

        var startTime = DateTime.UtcNow;
        using var dbContext = new FollyDbContext(options, null, MockHttpContextAccessor.Object, ConnectionString);

        // open a transaction so we can test against the real db and implicitly rollback to avoid changing the db
        dbContext.Database.BeginTransaction();

        dbContext.Users.Add(TestUser);

        // save SaveChanges instead of SaveChangesAsync to avoid running the SaveChangesAsync override
        dbContext.SaveChanges();

        dbContext.Languages.Add(new Language { Id = -1, Name = "test language", CountryCode = "country", LanguageCode = "lang" });
        await dbContext.SaveChangesAsync();
        var savedLanguage = dbContext.Languages.FirstOrDefault(x => x.Id == -1);

        Assert.NotNull(savedLanguage);
        Assert.Equal(TestUser.Id, savedLanguage.CreatedUserId);
        Assert.True(savedLanguage.CreatedDate >= startTime);
    }
}
