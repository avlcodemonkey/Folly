using System.Data;
using System.Linq.Expressions;
using System.Text;
using Folly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Headers != null) {
            return request.Headers[_RequestedWithHeader] == _XmlHttpRequest;
        }

        return false;
    }

    /// <summary>
    /// Redirect to an action, without using magic strings.
    /// </summary>
    /// <typeparam name="TController">Type of controller to redirect to.</typeparam>
    /// <param name="controller">Controller to redirect to.</param>
    /// <param name="expression">Evaluates to name of action to invoke.</param>
    /// <returns>Redirect action result.</returns>
    public static IActionResult Redirect<TController>(this Controller controller, Expression<Func<TController, string>> expression, object? routeValues = null)
        where TController : Controller {
        if (expression.Body is not ConstantExpression constant) {
            throw new ArgumentException("Expression must be a constant expression.");
        }
        return controller.RedirectToAction(constant.ToString(), typeof(TController).Name.StripController(), routeValues);
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
    /// Toogle a boolean session setting.
    /// </summary>
    /// <param name="session">Session to modify.</param>
    /// <param name="settingName">Name of setting to toggle.</param>
    public static void ToggleSetting(this ISession session, string settingName)
        => session.SetString(settingName, (!session.GetString(settingName).ToBool()).ToString());

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
    /// Check if user has help enabled.
    /// </summary>
    /// <param name="httpContext">Current request context.</param>
    /// <returns>True if user enabled help, else false.</returns>
    public static bool WantsHelp(this HttpContext httpContext) => httpContext.Session.GetString(Help.SettingName).ToBool();
}
