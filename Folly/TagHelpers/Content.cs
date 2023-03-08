using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class ContentTagHelper : BaseTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("id", "content");
        base.Process(context, output);
    }
}
