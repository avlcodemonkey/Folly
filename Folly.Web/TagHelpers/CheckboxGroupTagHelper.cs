using System.Text.Encodings.Web;
using Folly.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// Creates an input group for a checkbox with label.
/// </summary>
/// <param name="htmlHelper">HtmlHelper for rendering.</param>
public sealed class CheckboxGroupTagHelper(IHtmlHelper htmlHelper) : GroupBaseTagHelper(htmlHelper) {
    private IHtmlContent BuildCheckbox(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName)) {
            return HtmlString.Empty;
        }

        var label = new TagBuilder("label");
        label.AddCssClass("form-checkbox");
        label.MergeAttribute("for", FieldName);

        var input = new TagBuilder("input");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.MergeAttribute(x.Name, x.Value.ToString()));

        input.AddCssClass("form-input");
        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);
        input.MergeAttribute("type", "checkbox", true);
        input.MergeAttribute("value", "true", true);
        input.SetAttributeIf("checked", "true", For?.ModelExplorer.Model?.ToString().ToBool() == true);

        label.InnerHtml.AppendHtml(input);
        if (!string.IsNullOrWhiteSpace(FieldTitle)) {
            label.InnerHtml.Append(FieldTitle);
        }

        return label;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildCheckbox(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(await BuildHelp());

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.Content.AppendHtml(BuildLabel(""));
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
