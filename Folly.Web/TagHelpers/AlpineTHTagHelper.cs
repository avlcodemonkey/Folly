using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTHTagHelper : TagHelper {
    public AlpineTHTagHelper() { }

    public string? Property { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        if (string.IsNullOrWhiteSpace(Property)) {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        output.TagName = "th";
        output.Attributes.SetAttribute(":class", $"sortClass('{Property}')");
        output.Attributes.SetAttribute("@click", $"onSortClick('{Property}')");
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
