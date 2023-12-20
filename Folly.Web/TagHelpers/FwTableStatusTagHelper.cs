using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// PartialTagHelper extension that builds the fw-table status.
/// </summary>
public sealed class FwTableStatusTagHelper : PartialTagHelper {
    /// <summary>
    /// Number of columns for the table.
    /// </summary>
    [HtmlAttributeName("colspan")]
    public int ColSpan { get; set; }

    public FwTableStatusTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
        => Name = "_FwTableStatus";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Model = ColSpan;
        await base.ProcessAsync(context, output);
    }
}
