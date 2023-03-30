namespace Folly.Configuration;

public sealed class AuthConfiguration {
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string Domain { get; set; } = "";
}
