using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FwTableRowTagHelper : TagHelper {
    public FwTableRowTagHelper() { }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var tr = new TagBuilder("tr");
        tr.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        output.TagName = "template";
        output.AddClass("table-row-template", HtmlEncoder.Default);
        output.Content.AppendHtml(tr);

        await base.ProcessAsync(context, output);
    }
}
