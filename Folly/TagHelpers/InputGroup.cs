using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class InputGroupTagHelper : GroupBaseTagHelper {
    private static readonly Type[] NumberTypes = { typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(int?), typeof(long?), typeof(decimal?), typeof(double?) };

    private IHtmlContent BuildInput() {
        if (string.IsNullOrWhiteSpace(FieldName))
            return HtmlString.Empty;

        var input = new TagBuilder("input");
        input.AddCssClass("form-input");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);

        var name = FieldName.ToLower(CultureInfo.InvariantCulture);
        var type = "text";
        if (name.EndsWith("password", StringComparison.InvariantCultureIgnoreCase))
            type = "password";
        else if (name.EndsWith("email", StringComparison.InvariantCultureIgnoreCase))
            type = "email";
        else if (name.EndsWith("date", StringComparison.InvariantCultureIgnoreCase))
            type = "date";
        else if (For != null && NumberTypes.Contains(For.ModelExplorer.ModelType))
            type = "number";

        input.Attributes.Add("type", type);
        input.Attributes.Add("value", type == "password" ? "" : For?.ModelExplorer.Model?.ToString());

        input.Attributes.AddIf("required", "true", (Required.HasValue && Required.Value) || (!Required.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.AddIf("autofocus", "true", Autofocus);
        input.Attributes.AddIf("disabled", "true", Disabled == true);
        input.Attributes.AddIf("readonly", "true", ReadOnly);

        if (For != null) {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(CultureInfo.InvariantCulture), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(CultureInfo.InvariantCulture), minLength > 0);
        }

        return input;
    }

    public InputGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool Autofocus { get; set; }
    public bool ReadOnly { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        output.TagName = "div";
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml(BuildLabel());
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
