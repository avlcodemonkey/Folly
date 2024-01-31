using System.Globalization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// Creates an input group for a input with label.
/// </summary>
/// <param name="htmlHelper">HtmlHelper for rendering.</param>
public sealed class InputGroupTagHelper(IHtmlHelper htmlHelper) : GroupBaseTagHelper(htmlHelper) {
    private static readonly Type[] _NumberTypes = [typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(int?), typeof(long?), typeof(decimal?), typeof(double?)];

    private IHtmlContent BuildInput(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName)) {
            return HtmlString.Empty;
        }

        var input = new TagBuilder("input");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.MergeAttribute(x.Name, x.Value.ToString()));

        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);

        var name = FieldName.ToLower(CultureInfo.InvariantCulture);
        var type = "text";
        if (name.EndsWith("password", StringComparison.InvariantCultureIgnoreCase)) {
            type = "password";
        } else if (name.EndsWith("email", StringComparison.InvariantCultureIgnoreCase)) {
            type = "email";
        } else if (name.EndsWith("date", StringComparison.InvariantCultureIgnoreCase)) {
            type = "date";
        } else if (For != null && _NumberTypes.Contains(For.ModelExplorer.ModelType)) {
            type = "number";
        }
        input.MergeAttribute("type", type, true);

        var value = For?.ModelExplorer.Model;
        if (value != null) {
            // if a date input, try to format value correctly for html to handle
            if (type == "date" && DateTime.TryParse(value.ToString(), out var dateValue)) {
                value = dateValue.ToString("yyyy-MM-dd");
            }
        }
        input.MergeAttribute("value", type == "password" ? "" : value?.ToString(), true);

        if (Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true)) {
            input.MergeAttribute("required", "true", true);
        }

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            if (maxLength > 0) {
                input.MergeAttribute("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), true);
            }
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            if (minLength > 0) {
                input.MergeAttribute("minLength", minLength.ToString(CultureInfo.InvariantCulture), true);
            }
        }

        return input;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(await BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.Content.AppendHtml(BuildLabel());
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
