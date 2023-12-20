using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// PartialTagHelper extension that builds the fw-table footer.
/// </summary>
public sealed class FwTableFooterTagHelper : PartialTagHelper {
    public FwTableFooterTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
        => Name = "_FwTableFooter";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        => await base.ProcessAsync(context, output);
}
