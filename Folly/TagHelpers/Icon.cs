using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class IconTagHelper : BaseTagHelper
{
    public IconTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public FollyIcon Name { get; set; }
    public string Type { get; set; } = "primary";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "i";
        output.Attributes.Add("class", $"pr pr-{Name.ToCssClass()} text-{Type}");
        base.Process(context, output);
    }
}
