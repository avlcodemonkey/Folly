using System.Text.RegularExpressions;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly.Extensions;

public static class TagHelperExtensions {
    private static readonly Regex _CssRegex = new(@"(?<!_)([A-Z])", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _CaseRegex = new(@"([a-z])([A-Z])", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    /// <summary>
    /// Merge an attribute if condition is met.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="set">Set (add or overwrite) attribute if true.</param>
    public static void SetAttributeIf(this TagBuilder tagBuilder, string key, string? value, bool set) {
        if (set) {
            tagBuilder.MergeAttribute(key, value, true);
        }
    }

    /// <summary>
    /// Convert an icon enum to a css class name.
    /// </summary>
    /// <param name="val">String value to convert.</param>
    /// <returns>Css class name string.</returns>
    public static string ToCssClass(this Icon val) => _CssRegex.Replace(val.ToString(), "-$1").Trim('-').ToLower();

    /// <summary>
    /// Convert a pascal case string to hyphen case. IE "QuickBrownFoxJumpsOverTheLazyDog" to "quick-brown-fox-jumps-over-the-lazy-dog"
    /// </summary>
    /// <param name="value">Toggle enum value to convert.</param>
    /// <returns>Converted string.</returns>
    public static string ToHyphenCase(this DataToggle toggle) => _CaseRegex.Replace(toggle.ToString(), "$1-$2").ToLower();
}
