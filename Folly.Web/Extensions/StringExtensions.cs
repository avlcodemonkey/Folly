namespace Folly.Extensions;

public static class StringExtensions {
    /// <summary>
    /// Converts a string value to a boolean. Default to false.
    /// </summary>
    /// <param name="val">Value to attempt to convert.</param>
    /// <returns>Bool value</returns>
    public static bool ToBool(this string? val) => val != null && (val == "1" || val.ToLower() == "true");

    /// <summary>
    /// Uppercase the first character of a string.
    /// </summary>
    /// <param name="value">String to update.</param>
    /// <returns>Updated string.</returns>
    public static string UppercaseFirst(this string value) => string.IsNullOrWhiteSpace(value) ? "" : char.ToUpper(value[0]) + value[1..];
}
