namespace Folly.Utils;

/// <summary>
/// Provides helper methods for working with user names.
/// </summary>
public sealed class NameHelper {
    /// <summary>
    /// Formats the user name as "last, first" with no whitespace.
    /// </summary>
    /// <param name="firstName">User first name.</param>
    /// <param name="lastName">User last name.</param>
    /// <returns>Formatted name to display.</returns>
    public static string DisplayName(string? firstName = null, string? lastName = null)
        => string.Join(", ", new string[] { lastName ?? "", firstName ?? "" }.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));
}
