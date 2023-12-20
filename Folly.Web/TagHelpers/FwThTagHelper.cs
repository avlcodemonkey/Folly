using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FwThTagHelper : TagHelper {
    public FwThTagHelper() { }

    public string? Property { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "th";
        if (string.IsNullOrWhiteSpace(Property)) {
            output.AddClass("table-no-sort", HtmlEncoder.Default);
        } else {
            output.Attributes.SetAttribute("data-property", Property);
        }
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
