namespace Folly.Configuration;

public interface IAppConfiguration {
    AuthConfiguration Auth { get; }
    DatabaseConfiguration Database { get; }
    bool IsDevelopment { get; }
}
