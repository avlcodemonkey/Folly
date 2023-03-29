using System.Text.Encodings.Web;
using Folly.Models;
using Folly.Utils;
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
        output.Attributes.Add("method", Method.ToString());
        output.Attributes.Add("id", $"{Action.ToLower()}{Controller.UppercaseFirst()}Form");

        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
        output.Attributes.Add("action", urlHelper.Action(Action, Controller, RouteValues));

        output.Content.AppendHtml(HtmlHelper.AntiForgeryToken());
        output.Content.AppendHtml(output.GetChildContentAsync().Result);

        base.Process(context, output);
    }
}
