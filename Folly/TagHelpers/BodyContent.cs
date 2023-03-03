using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class BodyContentTagHelper : BaseTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("id", "bodyContent");
        output.AddClass("p-2", HtmlEncoder.Default);

        base.Process(context, output);
    }
}
