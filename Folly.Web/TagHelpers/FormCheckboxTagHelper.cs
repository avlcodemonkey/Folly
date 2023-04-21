using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FormCheckboxTagHelper : BaseTagHelper {
    public FormCheckboxTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public string? Id { get; set; }
    public bool Checked { get; set; }
    public string Label { get; set; } = "";
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var id = string.IsNullOrWhiteSpace(Id) ? $"{Name}_{Value}" : Id;
        var label = new TagBuilder("label");
        label.MergeAttribute("for", id);

        var input = new TagBuilder("input");
        input.MergeAttribute("id", id, true);
        input.MergeAttribute("name", Name);
        input.MergeAttribute("type", "checkbox");
        input.MergeAttribute("value", Value);
        input.SetAttributeIf("checked", "true", Checked);

        label.InnerHtml.AppendHtml(input);
        label.InnerHtml.Append(Label);

        output.Content.AppendHtml(label);

        await base.ProcessAsync(context, output);
    }
}
