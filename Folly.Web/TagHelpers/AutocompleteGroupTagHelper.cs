using System.Globalization;
using Folly.Extensions;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// @todo this is not actually working yet.  
/// </summary>
/// <param name="htmlHelper"></param>
public sealed class AutocompleteGroupTagHelper(IHtmlHelper htmlHelper) : GroupBaseTagHelper(htmlHelper) {
    private string AutoCompleteName => $"{FieldName}_AutoComplete";

    private IHtmlContent BuildInput(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName)) {
            return HtmlString.Empty;
        }

        var input = new TagBuilder("input");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.MergeAttribute(x.Name, x.Value.ToString()));

        input.AddCssClass("form-input");
        input.MergeAttribute("id", AutoCompleteName, true);
        input.MergeAttribute("name", AutoCompleteName, true);
        input.MergeAttribute("type", "text", true);
        input.MergeAttribute("placeholder", Core.StartTyping, true);
        input.Attributes.Add("autocomplete", "off");
        input.Attributes.Add("data-autocomplete-display", "");

        return input;
    }

    private TagBuilder BuildHidden() {
        var input = new TagBuilder("input");
        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);
        input.MergeAttribute("type", "hidden", true);
        input.MergeAttribute("value", For?.ModelExplorer.Model?.ToString(), true);
        input.SetAttributeIf("required", "true", Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.Add("data-autocomplete-value", "");

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.SetAttributeIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }

        return input;
    }

    public string SrcUrl { get; set; } = "";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildHidden());
        inputGroup.InnerHtml.AppendHtml(BuildInput(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(await BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        var autocomplete = new TagBuilder("nilla-autocomplete");
        autocomplete.SetAttributeIf("data-src-url", SrcUrl, !string.IsNullOrWhiteSpace(SrcUrl));
        autocomplete.Attributes.Add("data-empty-message", Core.AutocompleteNoMatches);
        autocomplete.InnerHtml.AppendHtml(BuildLabel(AutoCompleteName));
        autocomplete.InnerHtml.AppendHtml(inputGroup);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.Content.AppendHtml(autocomplete);

        await base.ProcessAsync(context, output);
    }
}
