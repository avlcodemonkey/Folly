using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class FormCheckboxTagHelper : BaseTagHelper
{
    public FormCheckboxTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool? Disabled { get; set; }
    public string Id { get; set; }
    public bool IsChecked { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var id = Id.IsEmpty() ? $"{Name}_{Value}" : Id;
        var label = new TagBuilder("label");
        label.Attributes.Add("for", id);

        var input = new TagBuilder("input");
        input.Attributes.Add("id", id);
        input.Attributes.Add("name", Name);
        input.Attributes.Add("type", "checkbox");
        input.Attributes.Add("value", Value);
        input.Attributes.AddIf("checked", "true", IsChecked);
        input.Attributes.AddIf("disabled", "true", Disabled == true);

        label.InnerHtml.AppendHtml(input);
        label.InnerHtml.Append(Label);

        output.Content.AppendHtml(label);

        base.Process(context, output);
    }
}
