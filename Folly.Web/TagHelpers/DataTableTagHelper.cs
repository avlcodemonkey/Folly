using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class DataTableTagHelper : BaseTagHelper {
    public DataTableTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public string? Key { get; set; }

    public string? Src { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        if (string.IsNullOrWhiteSpace(Key) || string.IsNullOrWhiteSpace(Src)) {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        var table = new TagBuilder("table");
        table.AddCssClass("col");
        table.AddCssClass("striped");
        table.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        var sortAscSpan = new TagBuilder("span");
        sortAscSpan.AddCssClass("sort-icon");
        sortAscSpan.AddCssClass("sort-asc");
        sortAscSpan.Attributes.Add("aria-label", Core.Ascending);
        sortAscSpan.InnerHtml.AppendHtml(await HtmlHelper!.PartialAsync("Icons/_CaretUp"));
        var sortAscTemplate = new TagBuilder("template");
        sortAscTemplate.AddCssClass("sort-asc-template");
        sortAscTemplate.InnerHtml.AppendHtml(sortAscSpan);

        var sortDescSpan = new TagBuilder("span");
        sortDescSpan.AddCssClass("sort-icon");
        sortDescSpan.AddCssClass("sort-desc");
        sortDescSpan.Attributes.Add("aria-label", Core.Descending);
        sortDescSpan.InnerHtml.AppendHtml(await HtmlHelper!.PartialAsync("Icons/_CaretDown"));
        var sortDescTemplate = new TagBuilder("template");
        sortDescTemplate.AddCssClass("sort-desc-template");
        sortDescTemplate.InnerHtml.AppendHtml(sortDescSpan);

        var rowDiv = new TagBuilder("div");
        rowDiv.AddCssClass("row");
        rowDiv.InnerHtml.AppendHtml(sortAscTemplate);
        rowDiv.InnerHtml.AppendHtml(sortDescTemplate);
        rowDiv.InnerHtml.AppendHtml(table);

        var containerDiv = new TagBuilder("div");
        containerDiv.AddCssClass("container");
        containerDiv.InnerHtml.AppendHtml(await HtmlHelper!.PartialAsync("_DataTableHeader"));
        containerDiv.InnerHtml.AppendHtml(rowDiv);
        containerDiv.InnerHtml.AppendHtml(await HtmlHelper!.PartialAsync("_DataTableFooter"));

        output.TagName = "x-table";
        output.Attributes.SetAttribute("data-key", Key);
        output.Attributes.SetAttribute("data-src", Src);
        output.Content.AppendHtml(containerDiv);

        await base.ProcessAsync(context, output);
    }
}
