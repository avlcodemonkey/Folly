using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class AuthorizedMenuItemTagHelper : BaseTagHelper
{
    readonly IHttpContextAccessor HttpContextAccessor;
    readonly IUrlHelperFactory UrlHelperFactory;

    public AuthorizedMenuItemTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        HttpContextAccessor = httpContextAccessor;
        UrlHelperFactory = urlHelperFactory;
    }

    public string? Action { get; set; }
    public string? Controller { get; set; }
    public bool? HasAccess { get; set; }
    public Icon Icon { get; set; }
    public string Title { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        output.TagMode = TagMode.StartTagAndEndTag;
        if (Controller.IsEmpty() || Action.IsEmpty() || HasAccess == false || HttpContextAccessor.HttpContext?.User.HasAccess(Controller!, Action!, HttpVerb.Get) != true)
        {
            NoRender = true;
            base.Process(context, output);
            return;
        }

        output.TagName = "li";

        var a = new TagBuilder("a");
        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
        a.Attributes.Add("href", urlHelper.Action(Action, Controller));

        var i = new TagBuilder("i");
        i.AddCssClass("fl");
        i.AddCssClass("fl-lg");
        i.AddCssClass($"fl-{Icon.ToCssClass()}");
        a.InnerHtml.AppendHtml(i);

        var span = new TagBuilder("span");
        span.InnerHtml.Append(Title);
        a.InnerHtml.AppendHtml(span);

        output.Content.AppendHtml(a);
        output.Content.AppendHtml(output.GetChildContentAsync().Result);

        base.Process(context, output);
    }
}
