using System.Data;
using System.Text;
using Folly.Controllers;
using Folly.Models;
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
        ArgumentNullException.ThrowIfNull(request);

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
    public static string StripController(this string value) => value[..value.LastIndexOf("Controller")];

    /// <summary>
    /// Convert the ModelStateDictionary into a string of errors a view can display.
    /// </summary>
    /// <param name="state">State of a model.</param>
    /// <returns>Space separated list of errors.</returns>
    public static string ToErrorString(this ModelStateDictionary state)
        => string.Join(" <br />", state.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray());
    /// <summary>
    /// Convert IEnumerable to a list of select list items.
    /// </summary>
    /// <typeparam name="T">Enumerable type.</typeparam>
    /// <param name="enumerable">List of items to convert.</param>
    /// <param name="text">Function to get the option text.</param>
    /// <param name="value">Funciton to get the option value.</param>
    /// <returns>List of select list items.</returns>
    public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text, Func<T, string> value)
        => enumerable.Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();

    /// <summary>
    /// <summary>
    /// Adds an error message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="errorMsg">Error message to add.</param>
    public static void AddError(this ViewDataDictionary viewData, ModelStateDictionary modelState) {
        ArgumentNullException.ThrowIfNull(modelState);

        viewData.AddError(modelState.ToErrorString());
    }

    /// <summary>
    /// Adds an error message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="error">Error message to add.</param>
    public static void AddError(this ViewDataDictionary viewData, string error) {
        ArgumentNullException.ThrowIfNull(error);

        viewData[BaseController.ErrorProperty] = error;
    }

    /// <summary>
    /// Adds a message to the ViewData dictionary.
    /// </summary>
    /// <param name="viewData">ViewData to update.</param>
    /// <param name="message">Message to add.</param>
    public static void AddMessage(this ViewDataDictionary viewData, string message) {
        ArgumentNullException.ThrowIfNull(message);

        viewData[BaseController.MessageProperty] = message;
    }
}
