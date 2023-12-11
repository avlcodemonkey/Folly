using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTHTagHelper : TagHelper {
    public AlpineTHTagHelper() { }

    public string? Property { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "th";
        if (string.IsNullOrWhiteSpace(Property)) {
            output.AddClass("alpine-no-sort", HtmlEncoder.Default);
        } else {
            output.Attributes.SetAttribute(":class", $"sortClass('{Property}')");
            output.Attributes.SetAttribute("@click", $"onSortClick('{Property}')");
        }
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
