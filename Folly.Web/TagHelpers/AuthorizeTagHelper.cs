using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// Supresses output if the user doesn't have the specified role(s).
/// </summary>
[HtmlTargetElement(Attributes = "authorize-roles")]
public sealed class AuthorizeTagHelper : TagHelper {
    private readonly IHttpContextAccessor _HttpContextAccessor;

    public AuthorizeTagHelper(IHttpContextAccessor httpContextAccessor) => _HttpContextAccessor = httpContextAccessor;

    [HtmlAttributeName("authorize-roles")]
    public string Roles { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (output.Attributes.TryGetAttribute("authorize-roles", out var attribute)) {
            output.Attributes.Remove(attribute);
        }

        var user = _HttpContextAccessor.HttpContext?.User;
        if (user == null || !Roles.Split(',').Select(x => x.Trim().ToLower(CultureInfo.InvariantCulture)).Any(user.IsInRole)) {
            output.SuppressOutput();
        }
    }
}
