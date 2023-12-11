using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// PartialTagHelper extension that build the alpine table status section.
/// </summary>
public sealed class AlpineTableStatusTagHelper : PartialTagHelper {
    /// <summary>
    /// Number of columns for the table.
    /// </summary>
    [HtmlAttributeName("colspan")]
    public int ColSpan { get; set; }

    public AlpineTableStatusTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope) {
        Name = "_AlpineTableStatus";
        Model = ColSpan;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        => await base.ProcessAsync(context, output);
}
