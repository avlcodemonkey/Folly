using System.Globalization;
using System.Text.Encodings.Web;
using Folly.Constants;
using Folly.Extensions;
using Folly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FormContentTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : BaseTagHelper(htmlHelper) {
    private readonly IUrlHelperFactory _UrlHelperFactory = urlHelperFactory;

    public string Action { get; set; } = "";
    public string Controller { get; set; } = "";
    public object? For { get; set; }
    public HttpVerb Method { get; set; } = HttpVerb.Post;
    public object? RouteValues { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        if (For != null) {
            Controller = For.GetType().Name;
            Action = ((BaseModel)For).FormAction;
            Method = ((BaseModel)For).FormMethod;
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "form";
        output.AddClass("container", HtmlEncoder.Default);
        output.Attributes.SetAttribute("method", Method.ToString());
        output.Attributes.SetAttribute("id", $"{Action!.ToLower(CultureInfo.InvariantCulture)}{Controller.UppercaseFirst()}Form");

        var urlHelper = _UrlHelperFactory.GetUrlHelper(HtmlHelper!.ViewContext);
        output.Attributes.SetAttribute("action", urlHelper.Action(Action, Controller, RouteValues));

        output.Content.AppendHtml(HtmlHelper.AntiForgeryToken());
        output.Content.AppendHtml(output.GetChildContentAsync().Result);

        await base.ProcessAsync(context, output);
    }
}
