using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// PartialTagHelper extension that build the alpine table footer.
/// </summary>
public sealed class AlpineTableFooterTagHelper : PartialTagHelper {
    public AlpineTableFooterTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
        => Name = "_AlpineTableFooter";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        => await base.ProcessAsync(context, output);
}
