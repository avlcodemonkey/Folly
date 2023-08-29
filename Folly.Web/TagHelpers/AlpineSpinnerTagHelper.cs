using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineSpinnerTagHelper : TagHelper {
    public AlpineSpinnerTagHelper() { }

    public int Colspan { get; set; } = 1;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var div = new TagBuilder("div");
        div.AddCssClass("spinner");

        var h1 = new TagBuilder("h1");
        h1.AddCssClass("text-center");
        h1.InnerHtml.AppendHtml(div);

        var td = new TagBuilder("td");
        td.MergeAttribute("colspan", Colspan.ToString(CultureInfo.InvariantCulture));
        td.InnerHtml.AppendHtml(h1);

        var tr = new TagBuilder("tr");
        tr.InnerHtml.AppendHtml(td);

        output.TagName = "template";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("x-if", "!rows?.length");
        output.Content.AppendHtml(tr);

        await base.ProcessAsync(context, output);
    }
}
