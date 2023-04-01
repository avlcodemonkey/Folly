using System.Globalization;
using System.Text.Encodings.Web;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AutocompleteGroupTagHelper : GroupBaseTagHelper {
    private string AutoCompleteName => $"{FieldName}_AutoComplete";

    private IHtmlContent BuildInput() {
        var input = new TagBuilder("input");
        input.AddCssClass("form-input");
        input.Attributes.Add("id", AutoCompleteName);
        input.Attributes.Add("name", AutoCompleteName);
        input.Attributes.Add("type", "text");
        input.Attributes.AddIf("value", DefaultText, !DefaultText.IsEmpty());
        input.Attributes.AddIf("required", "true", (Required.HasValue && Required.Value) || (!Required.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.AddIf("autofocus", "true", Autofocus);

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }
        input.Attributes.AddIf("data-url", Url, !Url.IsEmpty());
        input.Attributes.AddIf("data-params", Params, !Params.IsEmpty());
        input.Attributes.AddIf("data-preload", "true", Preload);
        input.Attributes.Add("placeholder", Core.StartTyping);

        return input;
    }

    private IHtmlContent BuildHidden() {
        var input = new TagBuilder("input");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);
        input.Attributes.Add("type", "hidden");
        input.Attributes.Add("value", For?.ModelExplorer.Model?.ToString());
        input.Attributes.AddIf("required", "true", (Required.HasValue && Required.Value) || (!Required.HasValue && For?.Metadata.IsRequired == true));

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }

        return input;
    }

    public AutocompleteGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool Autofocus { get; set; }
    public string Params { get; set; } = "";
    public bool Preload { get; set; }
    public string Url { get; set; } = "";
    public string DefaultText { get; set; } = "";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildHidden());
        inputGroup.InnerHtml.AppendHtml(BuildInput());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        output.TagName = "div";
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml(BuildLabel(AutoCompleteName));
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
