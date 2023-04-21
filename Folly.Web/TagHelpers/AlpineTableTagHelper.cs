using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTableTagHelper : TagHelper {
    public AlpineTableTagHelper() { }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var table = new TagBuilder("table");
        table.AddCssClass("col");
        table.AddCssClass("striped");
        table.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        output.TagName = "div";
        output.AddClass("row", HtmlEncoder.Default);
        output.Content.AppendHtml(table);

        await base.ProcessAsync(context, output);
    }
}
