using System.Text.Encodings.Web;
using Folly.Models;
using Folly.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class HorizontalFormTagHelper : BaseTagHelper
{
    readonly IUrlHelperFactory UrlHelperFactory;

    public HorizontalFormTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        UrlHelperFactory = urlHelperFactory;
    }

    public string Action { get; set; }
    public string Controller { get; set; }
    public object For { get; set; }
    public bool IsFullWidth { get; set; }
    public HttpVerb Method { get; set; } = HttpVerb.Post;
    public object RouteValues { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        if (For != null)
        {
            Controller = For.GetType().Name;
            Action = ((BaseModel)For).FormAction;
            Method = ((BaseModel)For).FormMethod;
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "form";
        output.AddClass("container", HtmlEncoder.Default);
        output.AddClass("form-horizontal", HtmlEncoder.Default);
        output.AddClass("p-5", HtmlEncoder.Default);
        output.Attributes.Add("method", Method.ToString());
        output.Attributes.Add("id", $"{Action.ToLower()}{Controller.UppercaseFirst()}Form");
        output.Attributes.Add("data-confirm", Core.DiscardChanges);
        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
        output.Attributes.Add("action", urlHelper.Action(Action, Controller, RouteValues));

        var col = new TagBuilder("div");
        if (IsFullWidth)
        {
            col.AddCssClass("col-12");
        }
        else
        {
            col.AddCssClass("col-6");
            col.AddCssClass("col-xl-9");
        }
        col.InnerHtml.AppendHtml(HtmlHelper.AntiForgeryToken());
        col.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        var columns = new TagBuilder("div");
        columns.AddCssClass("columns");
        columns.InnerHtml.AppendHtml(col);

        output.Content.AppendHtml(columns);

        base.Process(context, output);
    }
}
