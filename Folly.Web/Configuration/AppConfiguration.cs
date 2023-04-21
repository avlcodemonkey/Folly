namespace Folly.Configuration;

public sealed class AppConfiguration : IAppConfiguration {
    public AuthConfiguration Auth { get; set; } = new AuthConfiguration();
    public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();
    public bool IsDevelopment { get; set; }
}
