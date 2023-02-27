namespace Folly.Configuration;

public class AppConfiguration : IAppConfiguration
{
    public AuthConfiguration Auth { get; set; }
    public DatabaseConfiguration Database { get; set; }
    public bool IsDevelopment { get; set; }
}
