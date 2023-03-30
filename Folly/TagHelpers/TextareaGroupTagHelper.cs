using System.Globalization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class TextareaGroupTagHelper : GroupBaseTagHelper {
    private IHtmlContent BuildInput() {
        var textarea = new TagBuilder("textarea");
        textarea.AddCssClass("form-input");
        textarea.Attributes.Add("id", FieldName);
        textarea.Attributes.Add("name", FieldName);
        textarea.Attributes.AddIf("required", "true", Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true));
        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            textarea.Attributes.AddIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            textarea.Attributes.AddIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }
        textarea.Attributes.AddIf("rows", Rows.ToString(CultureInfo.InvariantCulture), Rows > 0);
        textarea.InnerHtml.Append(For?.ModelExplorer.Model?.ToString() ?? "");
        return textarea;
    }

    public TextareaGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public int Rows { get; set; } = 4;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var div = new TagBuilder("div");
        div.AddCssClass("mb-1");
        div.InnerHtml.AppendHtml(BuildLabel());

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);
        div.InnerHtml.AppendHtml(inputGroup);

        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml(div);

        await base.ProcessAsync(context, output);
    }
}
