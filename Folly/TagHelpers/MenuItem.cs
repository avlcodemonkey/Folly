using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class MenuItemTagHelper : BaseTagHelper
{
    readonly IHttpContextAccessor HttpContextAccessor;
    readonly IUrlHelperFactory UrlHelperFactory;

    public MenuItemTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        HttpContextAccessor = httpContextAccessor;
        UrlHelperFactory = urlHelperFactory;
    }

    public string? Action { get; set; }
    public string? Controller { get; set; }
    public bool? HasAccess { get; set; }
    public Icon Icon { get; set; }
    public string Title { get; set; } = "";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        if (string.IsNullOrWhiteSpace(Controller) || string.IsNullOrWhiteSpace(Action) || HasAccess == false || HttpContextAccessor.HttpContext?.User.HasAccess(Controller, Action, HttpVerb.Get) != true)
        {
            output.SuppressOutput();
            await base.ProcessAsync(context, output);
            return;
        }

        var a = new TagBuilder("a");
        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper!.ViewContext);
        a.Attributes.Add("href", urlHelper.Action(Action, Controller));

        var i = new TagBuilder("i");
        i.AddCssClass("fl");
        i.AddCssClass("fl-lg");
        i.AddCssClass($"fl-{Icon.ToCssClass()}");
        a.InnerHtml.AppendHtml(i);

        var span = new TagBuilder("span");
        span.InnerHtml.Append(Title);
        a.InnerHtml.AppendHtml(span);

        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.AppendHtml(a);
        output.Content.AppendHtml(await output.GetChildContentAsync());

        await base.ProcessAsync(context, output);
    }
}
