using System.Security.Principal;
using Folly.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Folly.Domain.Tests;

/// <summary>
/// See https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database
/// for more details about using a database fixture for testing with xUnit.
/// </summary>
public class TestDatabaseFixture : IDisposable {
    private const string ConnectionString = "Filename=:memory:";
    private readonly SqliteConnection Connection;
    private readonly Mock<IConfiguration> MockConfiguration;

    private static readonly object Lock = new();
    private static bool DatabaseInitialized;

    public TestDatabaseFixture() {
        MockConfiguration = new Mock<IConfiguration>();
        MockConfiguration.Setup(x => x.GetSection("App").GetSection("Database")["FilePath"]).Returns(ConnectionString);

        // creates the SQLite in-memory database, which will persist until the connection is closed at the end of the test (see Dispose below).
        Connection = new SqliteConnection("Filename=:memory:");
        Connection.Open();

        // lock allows us to use this fixture safely with multiple classes of tests if needed
        lock (Lock) {
            if (!DatabaseInitialized) {
                // create the schema and user data we will need for each test
                using (var dbContext = CreateContext()) {
                    dbContext.Database.Migrate();
                    dbContext.Users.Add(UserForCreate);
                    dbContext.Users.Add(UserForUpdate);
                    dbContext.SaveChanges();
                }

                DatabaseInitialized = true;
            }
        }
    }
    private static IHttpContextAccessor CreateHttpContextAccessor(User? user = null) {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        if (user != null)
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity).Returns(new GenericIdentity(user.UserName, "test"));
        return mockHttpContextAccessor.Object;
    }

    public FollyDbContext CreateContext(User? user = null)
        => new(new DbContextOptionsBuilder<FollyDbContext>().UseSqlite(Connection).Options, MockConfiguration.Object, CreateHttpContextAccessor(user));

    public FollyDbContext CreateContextForCreate() => CreateContext(UserForCreate);

    public FollyDbContext CreateContextForUpdate() => CreateContext(UserForUpdate);

    public User UserForCreate { get; } = new() { Id = -1, UserName = "create_user", Email = "create_user@fake.com", LanguageId = -1, FirstName = "Create" };
    public User UserForUpdate { get; } = new() { Id = -2, UserName = "update_user", Email = "update_user@fake.com", LanguageId = -1, FirstName = "Update" };

    public void Dispose() {
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
