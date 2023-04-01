using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTableRowTagHelper : TagHelper {
    public AlpineTableRowTagHelper() { }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var tr = new TagBuilder("tr");
        tr.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        output.TagName = "template";
        output.Attributes.Add("x-for", "row in filteredRows");
        output.Attributes.Add(":key", "row._index");
        output.Content.AppendHtml(tr);

        await base.ProcessAsync(context, output);
    }
}
