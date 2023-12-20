using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FwTableTagHelper : TagHelper {
    public FwTableTagHelper() { }

    public string? Key { get; set; }

    public string? Src { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        if (string.IsNullOrWhiteSpace(Key) || string.IsNullOrWhiteSpace(Src)) {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        var div = new TagBuilder("div");
        div.AddCssClass("container");
        div.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        output.TagName = "fw-table";
        output.Attributes.SetAttribute("data-key", Key);
        output.Attributes.SetAttribute("data-src", Src);
        output.Content.AppendHtml(div);

        await base.ProcessAsync(context, output);
    }
}
