using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class AuthorizedButtonTagHelper : BaseTagHelper
{
    readonly IHttpContextAccessor HttpContextAccessor;
    readonly IUrlHelperFactory UrlHelperFactory;

    public AuthorizedButtonTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        HttpContextAccessor = httpContextAccessor;
        UrlHelperFactory = urlHelperFactory;
    }

    public string Action { get; set; }
    public FollyButton Class { get; set; } = FollyButton.BtnPrimary;
    public string Confirm { get; set; }
    public string Controller { get; set; }
    public bool ForceReload { get; set; } = false;
    public bool? HasAccess { get; set; }
    public string Role { get; set; }
    public object RouteValues { get; set; }
    public string Target { get; set; }
    public string Title { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        output.TagMode = TagMode.StartTagAndEndTag;
        if (!(HasAccess ?? HttpContextAccessor.HttpContext.User.HasAccess(Controller, Action, HttpVerb.Get)))
        {
            NoRender = true;
            base.Process(context, output);
            return;
        }

        output.TagName = "a";
        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
        output.Attributes.Add("href", urlHelper.Action(Action, Controller, RouteValues));
        output.Attributes.Add("data-method", "GET");
        output.Attributes.Add("title", Title);
        output.Attributes.AddIf("target", Target, !Target.IsEmpty());
        output.Attributes.AddIf("role", Role, !Role.IsEmpty());
        output.Attributes.AddIf("data-reload", "true", ForceReload);
        output.Attributes.AddIf("data-confirm", Confirm, !Confirm.IsEmpty());
        output.Content.Append(Title);

        var classList = new List<string> { "btn", "mr-2" };
        classList.Add(Class.ToCssClass());
        output.Attributes.Add("class", classList.Join(" "));

        base.Process(context, output);
    }
}
