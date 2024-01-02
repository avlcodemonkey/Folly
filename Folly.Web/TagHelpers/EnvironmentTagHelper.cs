using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

[HtmlTargetElement(Attributes = "data-environment")]
public sealed class EnvironmentTagHelper(IWebHostEnvironment environment) : TagHelper {
    private readonly IWebHostEnvironment _Environment = environment;

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var environment = _Environment.EnvironmentName.ToLower();
        output.Attributes.SetAttribute("data-environment", environment);
    }
}
