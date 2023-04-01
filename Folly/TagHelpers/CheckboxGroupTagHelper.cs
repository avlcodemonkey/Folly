using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class CheckboxGroupTagHelper : GroupBaseTagHelper {
    private IHtmlContent BuildCheckbox(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName))
            return HtmlString.Empty;

        var label = new TagBuilder("label");
        label.AddCssClass("form-checkbox");
        label.Attributes.Add("for", FieldName);

        var input = new TagBuilder("input");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.Attributes.Add(x.Name, x.Value.ToString()));

        input.AddCssClass("form-input");
        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);
        input.MergeAttribute("type", "checkbox", true);
        input.MergeAttribute("value", "true", true);
        input.SetAttributeIf("checked", "true", For?.ModelExplorer.Model?.ToString().ToBool() == true);

        label.InnerHtml.AppendHtml(input);
        if (!string.IsNullOrWhiteSpace(FieldTitle))
            label.InnerHtml.Append(FieldTitle);

        return label;
    }

    public CheckboxGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildCheckbox(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(BuildHelp());

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.Content.AppendHtml(BuildLabel(""));
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}