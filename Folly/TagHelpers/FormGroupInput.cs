using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class FormGroupInputTagHelper : FormBaseTagHelper
{
    static readonly Type[] NumberTypes = { typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(int?), typeof(long?), typeof(decimal?), typeof(double?) };

    IHtmlContent BuildInput()
    {
        var input = new TagBuilder("input");
        input.AddCssClass("form-input");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);

        var name = FieldName.ToLower();
        var type = "text";
        if (name.EndsWith("password"))
            type = "password";
        else if (name.EndsWith("email"))
            type = "email";
        else if (name.EndsWith("date"))
            type = "date";
        else if (For != null && NumberTypes.Contains(For.ModelExplorer.ModelType))
            type = "number";

        input.Attributes.Add("type", type);
        input.Attributes.Add("value", type == "password" ? "" : For?.ModelExplorer.Model?.ToString());

        input.Attributes.AddIf("required", "true", (IsRequired.HasValue && IsRequired.Value) || (!IsRequired.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.AddIf("autofocus", "true", Autofocus);
        input.Attributes.AddIf("disabled", "true", Disabled == true);
        input.Attributes.AddIf("readonly", "true", IsReadOnly);

        if (For != null)
        {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(), minLength > 0);
        }

        return input;
    }

    public FormGroupInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool Autofocus { get; set; }
    public string Match { get; set; }
    public string Params { get; set; }
    public bool Preload { get; set; }
    public string Target { get; set; }
    public DataToggle? Toggle { get; set; }
    public string Url { get; set; }
    public bool IsReadOnly { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        var div = new TagBuilder("div");
        div.AddCssClass("mb-1");
        div.InnerHtml.AppendHtml(BuildLabel());

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);
        div.InnerHtml.AppendHtml(inputGroup);

        output.Content.AppendHtml(div);

        base.Process(context, output);
    }
}
