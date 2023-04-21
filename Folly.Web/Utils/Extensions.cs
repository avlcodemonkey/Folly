using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Folly.Models;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Folly;

public static class Extensions {
    private const string RequestedWithHeader = "X-Requested-With";
    private const string XmlHttpRequest = "XMLHttpRequest";
    private static readonly Regex CaseRegex = new(@"([a-z])([A-Z])", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private static readonly Regex CssRegex = new(@"(?<!_)([A-Z])", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

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
    /// Helper for iterating over lists async.
    /// </summary>
    /// <typeparam name="T">Type of item in the list.</typeparam>
    /// <param name="list">List to iterate over.</param>
    /// <param name="func">Function to perform</param>
    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func) {
        foreach (var value in list) {
            await func(value);
        }
    }

    /// <summary>
    /// Check if the request object is an AJAX request.
    /// </summary>
    /// <param name="request">Current request object.</param>
    /// <returns>True if is an ajax request, else false.</returns>
    public static bool IsAjaxRequest(this HttpRequest request) {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (request.Headers != null)
            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        return false;
    }

    /// <summary>
    /// Check if a string is empty or null.
    /// </summary>
    /// <param name="value">String to check.</param>
    /// <returns>True if string is not null or empty.</returns>
    public static bool IsEmpty(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Redirect to an action, without using magic strings.
    /// </summary>
    /// <typeparam name="TController">Type of controller to redirect to.</typeparam>
    /// <param name="controller">Controller to redirect to.</param>
    /// <param name="expression">Evaluates to name of action to invoke.</param>
    /// <returns>Redirect action result.</returns>
    public static IActionResult Redirect<TController>(this Controller controller, Expression<Func<TController, string>> expression, object routeValues = null)
        where TController : Controller {
        if (expression.Body is not ConstantExpression constant)
            throw new ArgumentException("Expression must be a constant expression.");
        return controller.RedirectToAction(constant.Value.ToString(), typeof(TController).Name.StripController(), routeValues);
    }

    /// <summary>
    /// Strip the `Controller` keyword from the end of a string for use with redirects.
    /// </summary>
    /// <param name="value">Controller name to strip.</param>
    /// <returns>Shortened controller name.</returns>
    public static string StripController(this string value) => value[..value.LastIndexOf("Controller")];

    /// <summary>
    /// Converts a string value to a boolean. Default to false.
    /// </summary>
    /// <param name="val">Value to attempt to convert.</param>
    /// <returns>Bool value</returns>
    public static bool ToBool(this string? val) => val != null && (val == "1" || val.ToLower() == "true");

    /// <summary>
    /// Convert an icon enum to a css class name.
    /// </summary>
    /// <param name="val">String value to convert.</param>
    /// <returns>Css class name string.</returns>
    public static string ToCssClass(this Icon val) => CssRegex.Replace(val.ToString(), "-$1").Trim('-').ToLower();

    /// <summary>
    /// Convert the ModelStateDictionary into a string of errors a view can display.
    /// </summary>
    /// <param name="state">State of a model.</param>
    /// <returns>Space separated list of errors.</returns>
    public static string ToErrorString(this ModelStateDictionary state)
        => string.Join(" <br />", state.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray());

    /// <summary>
    /// Toogle a boolean session setting.
    /// </summary>
    /// <param name="session">Session to modify.</param>
    /// <param name="settingName">Name of setting to toggle.</param>
    public static void ToggleSetting(this ISession session, string settingName)
        => session.SetString(settingName, (!session.GetString(settingName).ToBool()).ToString());

    /// <summary>
    /// Convert a pascal case string to hyphen case. IE "QuickBrownFoxJumpsOverTheLazyDog" to "quick-brown-fox-jumps-over-the-lazy-dog"
    /// </summary>
    /// <param name="value">Toggle enum value to convert.</param>
    /// <returns>Converted string.</returns>
    public static string ToHyphenCase(this DataToggle toggle) => CaseRegex.Replace(toggle.ToString(), "$1-$2").ToLower();

    /// <summary>
    /// Convert IEnumerable to a list of select list items.
    /// </summary>
    /// <typeparam name="T">Enumerable type.</typeparam>
    /// <param name="enumerable">List of items to convert.</param>
    /// <param name="text">Function to get the option text.</param>
    /// <param name="value">Funciton to get the option value.</param>
    /// <returns>List of select list items.</returns>
    public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text, Func<T, string> value) => enumerable.Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();

    /// <summary>
    /// Uppercase the first character of a string.
    /// </summary>
    /// <param name="value">String to update.</param>
    /// <returns>Updated string.</returns>
    public static string UppercaseFirst(this string value) => value.IsEmpty() ? "" : (char.ToUpper(value[0]) + value[1..]);

    /// <summary>
    /// Check if user has help enabled.
    /// </summary>
    /// <param name="httpContext">Current request context.</param>
    /// <returns>True if user enabled help, else false.</returns>
    public static bool WantsHelp(this HttpContext httpContext) => httpContext.Session.GetString(Help.SettingName).ToBool();
}
