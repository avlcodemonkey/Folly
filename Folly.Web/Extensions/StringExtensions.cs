namespace Folly.Extensions;

public static class StringExtensions {
    /// <summary>
    /// Converts a string value to a boolean. Default to false.
    /// </summary>
    /// <param name="val">Value to attempt to convert.</param>
    /// <returns>Bool value</returns>
    public static bool ToBool(this string? val) => val != null && (val == "1" || val.Equals("true", StringComparison.InvariantCultureIgnoreCase));
}
