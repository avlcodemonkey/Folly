using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class CheckboxGroupTagHelper : GroupBaseTagHelper {
    private IHtmlContent BuildCheckbox() {
        var label = new TagBuilder("label");
        label.AddCssClass("form-checkbox");
        label.Attributes.Add("for", FieldName);
        label.Attributes.AddIf("disabled", "true", Disabled == true);

        var input = new TagBuilder("input");
        input.AddCssClass("form-input");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);
        input.Attributes.Add("type", "checkbox");
        input.Attributes.Add("value", "true");
        input.Attributes.AddIf("checked", "true", For?.ModelExplorer.Model?.ToString().ToBool() == true);
        input.Attributes.AddIf("disabled", "true", Disabled == true);

        label.InnerHtml.AppendHtml(input);
        if (!string.IsNullOrWhiteSpace(FieldTitle))
            label.InnerHtml.Append(FieldTitle);

        return label;
    }

    public CheckboxGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildCheckbox());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());

        output.TagName = "div";
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml(BuildLabel(""));
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
