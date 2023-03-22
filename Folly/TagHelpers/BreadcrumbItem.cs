using System.Text.Encodings.Web;
using Folly.Controllers;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class BreadcrumbItemTagHelper : BaseTagHelper
{
    public BreadcrumbItemTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public string? Action { get; set; }
    public string? Controller { get; set; }
    public bool IsActive { get; set; } = false;
    public string Label { get; set; } = "";
    public object? RouteValues { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        if (IsActive)
        {
            HtmlHelper!.ViewData[BaseController.TitleProperty] = Label;
            output.Content.Append(Label);

            if (!Label.IsEmpty() && HtmlHelper.ViewContext.HttpContext.Request.Headers.Any(x => x.Key == HtmxHeaders.Request))
            {
                // create a new title tag that htmx will swap out
                var title = new TagBuilder("title");
                title.Attributes.Add("id", "page-title");
                title.Attributes.Add("hx-swap-oob", "true");
                title.InnerHtml.Append(Label);
                output.PostContent.AppendHtml(title);
            }
        }
        else
        {
            output.Content.AppendHtml(HtmlHelper.ActionLink(Label, Action, Controller, RouteValues));
        }

        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;

        await base.ProcessAsync(context, output);
    }
}
