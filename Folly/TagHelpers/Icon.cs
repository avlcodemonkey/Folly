using System.Text.Encodings.Web;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class IconTagHelper : BaseTagHelper {
    public IconTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public Icon Name { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "i";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("fl", HtmlEncoder.Default);
        output.AddClass($"fl-{Name.ToCssClass()}", HtmlEncoder.Default);

        await base.ProcessAsync(context, output);
    }
}
