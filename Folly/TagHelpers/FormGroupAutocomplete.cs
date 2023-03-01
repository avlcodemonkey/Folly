using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class FormGroupAutocompleteTagHelper : FormBaseTagHelper
{
    string AutoCompleteName => $"{FieldName}_AutoComplete";

    IHtmlContent BuildInput()
    {
        var input = new TagBuilder("input");
        input.AddCssClass("form-input");
        input.Attributes.Add("id", AutoCompleteName);
        input.Attributes.Add("name", AutoCompleteName);
        input.Attributes.Add("type", "text");
        input.Attributes.AddIf("value", DefaultText, !DefaultText.IsEmpty());
        input.Attributes.AddIf("required", "true", (IsRequired.HasValue && IsRequired.Value) || (!IsRequired.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.AddIf("autofocus", "true", Autofocus);

        if (For != null)
        {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(), minLength > 0);
        }
        input.Attributes.Add("data-toggle", DataToggle.Autocomplete.ToHyphenCase());
        input.Attributes.Add("data-target", $"#{FieldName}");
        input.Attributes.AddIf("data-url", Url, !Url.IsEmpty());
        input.Attributes.AddIf("data-params", Params, !Params.IsEmpty());
        input.Attributes.AddIf("data-preload", "true", Preload);
        input.Attributes.AddIf("data-match", Match, Match != null);
        input.Attributes.Add("placeholder", Core.StartTyping);

        return input;
    }

    IHtmlContent BuildHidden()
    {
        var input = new TagBuilder("input");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);
        input.Attributes.Add("type", "hidden");
        input.Attributes.Add("value", For?.ModelExplorer.Model?.ToString());
        input.Attributes.AddIf("required", "true", (IsRequired.HasValue && IsRequired.Value) || (!IsRequired.HasValue && For?.Metadata.IsRequired == true));

        if (For != null)
        {
            var maxLength = GetMaxLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("maxlength", maxLength.ToString(), maxLength > 0);
            var minLength = GetMinLength(For.ModelExplorer.Metadata.ValidatorMetadata);
            input.Attributes.AddIf("minLength", minLength.ToString(), minLength > 0);
        }

        return input;
    }

    public FormGroupAutocompleteTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool Autofocus { get; set; }
    public string Match { get; set; }
    public string Params { get; set; }
    public bool Preload { get; set; }
    public string Url { get; set; }
    public string DefaultText { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        UseFormGroup(output);

        var div = BuildFormGroup();
        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildHidden());
        inputGroup.InnerHtml.AppendHtml(BuildInput());

        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        inputGroup.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);
        div.InnerHtml.AppendHtml(inputGroup);

        output.Content.AppendHtml(BuildLabel(AutoCompleteName));
        output.Content.AppendHtml(div);

        base.Process(context, output);
    }
}
