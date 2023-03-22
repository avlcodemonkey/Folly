using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTDTagHelper : TagHelper
{
    public AlpineTDTagHelper() { }

    public string? Property { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Property))
        {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("x-text", $"row.{Property}");
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
