using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class DoColumnTagHelper : BaseTagHelper
{
    public string Field { get; set; }
    public string Label { get; set; }
    public bool Sortable { get; set; } = true;
    public string SortDirection { get; set; }
    public int? SortOrder { get; set; }
    public string Width { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "td";
        output.Attributes.AddIf("data-width", Width, !Width.IsEmpty());
        output.Attributes.Add("data-sortable", Sortable.ToString());
        output.Attributes.Add("data-label", Label);
        output.Attributes.Add("data-field", Field);
        output.Attributes.AddIf("data-sort-dir", SortDirection, !SortDirection.IsEmpty());
        output.Attributes.AddIf("data-sort-order", SortOrder.ToString(), SortOrder.HasValue);
        base.Process(context, output);
    }
}
