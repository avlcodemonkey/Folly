using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class ContentTagHelper : BaseTagHelper {
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "div";
        output.Attributes.Add("id", "content");
        await base.ProcessAsync(context, output);
    }
}
