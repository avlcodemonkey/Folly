using System.Text.Encodings.Web;
using Folly.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class BreadcrumbItemTagHelper : BaseTagHelper
{
    readonly IUrlHelperFactory UrlHelperFactory;

    public BreadcrumbItemTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        UrlHelperFactory = urlHelperFactory;
    }

    public string Action { get; set; }
    public string Controller { get; set; }
    public bool IsActive { get; set; } = false;
    public string Label { get; set; }
    public object RouteValues { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "li";
        output.AddClass("breadcrumb-item", HtmlEncoder.Default);
        if (IsActive)
        {
            HtmlHelper.ViewData[BaseController.TitleProperty] = Label;
            output.Content.Append(Label);

            if (!Label.IsEmpty() && HtmlHelper.ViewContext.HttpContext.Request.Headers.Any(x => x.Key == "hx-request"))
            {
                // create a new title tag that htmx will swap out
                var title = new TagBuilder("title");
                title.Attributes.Add("id", "pageTitle");
                title.Attributes.Add("hx-swap-oob", "true");
                title.InnerHtml.Append(Label);
                output.PostContent.AppendHtml(title);
            }
        }
        else
        {
            output.Content.AppendHtml(HtmlHelper.ActionLink(Label, Action, Controller, RouteValues));
        }
        base.Process(context, output);
    }
}
