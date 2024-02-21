using System.Data;
using Folly.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Folly.Extensions;

public static class ControllerExtensions {
    private const string _RequestedWithHeader = "X-Requested-With";
    private const string _XmlHttpRequest = "XMLHttpRequest";

    /// <summary>
    /// Check if the request object is an AJAX request.
    /// </summary>
    /// <param name="request">Current request object.</param>
    /// <returns>True if is an ajax request, else false.</returns>
    public static bool IsAjaxRequest(this HttpRequest request) {
        if (request.Headers != null) {
            return request.Headers[_RequestedWithHeader] == _XmlHttpRequest;
        }

        return false;
    }

    /// <summary>
    /// Strip the `Controller` keyword from the end of a string for use with redirects.
    /// </summary>
    /// <param name="value">Controller name to strip.</param>
    /// <returns>Shortened controller name.</returns>
    public static string StripController(this string value) {
        var i = value.LastIndexOf("Controller", StringComparison.InvariantCultureIgnoreCase);
        return i > -1 ? value[..i] : value;
    }

    /// <summary>
    /// Convert IEnumerable to a list of select list items.
    /// </summary>
    /// <typeparam name="T">Enumerable type.</typeparam>
    /// <param name="enumerable">List of items to convert.</param>
    /// <param name="text">Function to get the option text.</param>
    /// <param name="value">Funciton to get the option value.</param>
    /// <returns>List of select list items.</returns>
    public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text, Func<T, string> value)
        => enumerable.Select(x => new SelectListItem { Text = text(x), Value = value(x) }).ToList();

    /// <summary>
    /// Convert the ModelStateDictionary into a string of errors and adds the error message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="errorMsg">Error message to add.</param>
    public static void AddError(this ViewDataDictionary viewData, ModelStateDictionary modelState) {
        ArgumentNullException.ThrowIfNull(modelState);

        var errors = modelState.Values.SelectMany(x => x.Errors).Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage));
        if (errors.Any()) {
            viewData.AddError(string.Join(" <br />", errors.Select(x => x.ErrorMessage).ToArray()));
        }
    }

    /// <summary>
    /// Adds an error message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="error">Error message to add.</param>
    public static void AddError(this ViewDataDictionary viewData, string error) {
        if (!string.IsNullOrWhiteSpace(error)) {
            viewData[ViewProperties.Error] = error;
        }
    }

    /// <summary>
    /// Adds a message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="message">Message to add.</param>
    public static void AddMessage(this ViewDataDictionary viewData, string message) {
        if (!string.IsNullOrWhiteSpace(message)) {
            viewData[ViewProperties.Message] = message;
        }
    }
}
