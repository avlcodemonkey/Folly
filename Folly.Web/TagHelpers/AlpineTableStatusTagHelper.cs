using System.Globalization;
using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class AlpineTableStatusTagHelper : TagHelper {
    public AlpineTableStatusTagHelper() { }

    public int Colspan { get; set; } = 1;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var div = new TagBuilder("div");
        div.AddCssClass("spinner");

        var loadingHeader = new TagBuilder("h1");
        loadingHeader.AddCssClass("text-center");
        loadingHeader.MergeAttribute("x-show", "isLoading");
        loadingHeader.InnerHtml.AppendHtml(div);

        var errorHeader = new TagBuilder("h4");
        errorHeader.AddCssClass("text-center");
        errorHeader.MergeAttribute("x-show", "hasError");
        errorHeader.InnerHtml.Append(Core.AlpineTableRequestFailed);

        var retryButton = new TagBuilder("button");
        retryButton.AddCssClass("button");
        retryButton.AddCssClass("primary");
        retryButton.MergeAttribute("title", Core.Retry);
        retryButton.MergeAttribute("@click", "onRetryClick");
        retryButton.InnerHtml.Append(Core.Retry);

        var errorP = new TagBuilder("p");
        errorP.AddCssClass("is-center");
        errorP.InnerHtml.AppendHtml(retryButton);

        var errorDiv = new TagBuilder("div");
        errorDiv.MergeAttribute("x-show", "hasError");
        errorDiv.InnerHtml.AppendHtml(errorHeader);
        errorDiv.InnerHtml.AppendHtml(errorP);

        var td = new TagBuilder("td");
        td.MergeAttribute("colspan", Colspan.ToString(CultureInfo.InvariantCulture));
        td.InnerHtml.AppendHtml(loadingHeader);
        td.InnerHtml.AppendHtml(errorDiv);

        var tr = new TagBuilder("tr");
        tr.InnerHtml.AppendHtml(td);

        output.TagName = "template";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("x-if", "isLoading || hasError");
        output.Content.AppendHtml(tr);

        await base.ProcessAsync(context, output);
    }
}
