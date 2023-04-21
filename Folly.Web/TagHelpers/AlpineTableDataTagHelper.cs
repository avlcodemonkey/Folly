using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTableDataTagHelper : TagHelper {
    public AlpineTableDataTagHelper() { }

    public string? Id { get; set; }

    public string? Src { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(Src)) {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        output.TagName = "div";
        output.AddClass("container", HtmlEncoder.Default);
        output.AddClass("alpine-table", HtmlEncoder.Default);
        output.Attributes.SetAttribute("x-data", $"table('{Id}', '{Src}')");
        output.Attributes.SetAttribute("x-cloak", null);
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
