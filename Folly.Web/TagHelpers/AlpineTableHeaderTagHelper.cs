using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// PartialTagHelper extension that build the alpine table header section.
/// </summary>
public sealed class AlpineTableHeaderTagHelper : PartialTagHelper {
    public AlpineTableHeaderTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
        => Name = "_AlpineTableHeader";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        => await base.ProcessAsync(context, output);
}
