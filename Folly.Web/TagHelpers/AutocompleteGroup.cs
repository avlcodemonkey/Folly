using System.Globalization;
using System.Text.Encodings.Web;
using Folly.Extensions;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AutocompleteGroupTagHelper : GroupBaseTagHelper {
    private string AutoCompleteName => $"{FieldName}_AutoComplete";

    private IHtmlContent BuildInput(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName))
            return HtmlString.Empty;

        var input = new TagBuilder("input");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.MergeAttribute(x.Name, x.Value.ToString()));

        input.AddCssClass("form-input");
        input.MergeAttribute("id", AutoCompleteName, true);
        input.MergeAttribute("name", AutoCompleteName, true);
        input.MergeAttribute("type", "text", true);
        input.MergeAttribute("placeholder", Core.StartTyping, true);
        input.SetAttributeIf("value", DefaultText, !DefaultText.IsEmpty());
        input.SetAttributeIf("required", "true", Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true));

        // @todo attributes like required and maxlength might need to be on the hidden input instead
        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }
        input.SetAttributeIf("data-url", Url, !Url.IsEmpty());
        input.SetAttributeIf("data-params", Params, !Params.IsEmpty());
        input.SetAttributeIf("data-preload", "true", Preload);

        return input;
    }

    private IHtmlContent BuildHidden() {
        var input = new TagBuilder("input");
        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);
        input.MergeAttribute("type", "hidden", true);
        input.MergeAttribute("value", For?.ModelExplorer.Model?.ToString(), true);
        input.SetAttributeIf("required", "true", Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true));

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }

        return input;
    }

    public AutocompleteGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public string Params { get; set; } = "";
    public bool Preload { get; set; }
    public string Url { get; set; } = "";
    public string DefaultText { get; set; } = "";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildHidden());
        inputGroup.InnerHtml.AppendHtml(BuildInput(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.Content.AppendHtml(BuildLabel(AutoCompleteName));
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
