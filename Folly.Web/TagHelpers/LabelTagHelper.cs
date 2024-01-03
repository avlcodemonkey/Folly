using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// Adds aria-label and title attributes.
/// </summary>
[HtmlTargetElement("a", Attributes = "label")]
public sealed class LabelTagHelper : TagHelper {
    public LabelTagHelper() { }

    [HtmlAttributeName("label")]
    public string Label { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (output.Attributes.TryGetAttribute("label", out var attribute)) {
            output.Attributes.Remove(attribute);
        }
        output.Attributes.Add("title", Label);
        output.Attributes.Add("aria-label", Label);
    }
}
