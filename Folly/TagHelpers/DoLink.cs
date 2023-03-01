using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class DoLinkTagHelper : BaseTagHelper
{
    readonly IHttpContextAccessor HttpContextAccessor;

    readonly IUrlHelperFactory UrlHelperFactory;

    internal TagBuilder InnerContent { get; set; }

    public DoLinkTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        HttpContextAccessor = httpContextAccessor;
        UrlHelperFactory = urlHelperFactory;
    }

    public string Action { get; set; }
    public string Class { get; set; } = "btn btn-link";
    public string Confirm { get; set; }
    public string Controller { get; set; }
    public bool? HasAccess { get; set; }
    public string IdProperty { get; set; } = "id";
    public string Method { get; set; } = "GET";
    public string ParentIdProperty { get; set; }
    public string Prompt { get; set; }
    public bool RenderWithoutAccess { get; set; } = false;
    public string TextProperty { get; set; }
    public string Title { get; set; }

    public HttpVerb GetMethod()
    {
        if (Method.ToUpper() == "POST")
            return HttpVerb.Post;
        if (Method.ToUpper() == "PUT")
            return HttpVerb.Put;
        if (Method.ToUpper() == "DELETE")
            return HttpVerb.Delete;
        return HttpVerb.Get;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        var hasAccess = HasAccess ?? HttpContextAccessor.HttpContext.User.HasAccess(Controller, Action, GetMethod());

        output.TagMode = TagMode.StartTagAndEndTag;
        if (!hasAccess)
        {
            if (RenderWithoutAccess)
            {
                output.TagName = "span";
                if (InnerContent != null)
                    output.Content.AppendHtml(InnerContent);
                else if (!TextProperty.IsEmpty())
                    output.Content.AppendHtml($"{{{{={TextProperty}}}}}");
            }
            else
            {
                NoRender = true;
            }
            base.Process(context, output);
            return;
        }

        output.TagName = "a";
        var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
        // ParentIdProperty
        var url = $"{urlHelper.Action(Action, Controller)}/";
        if (!ParentIdProperty.IsEmpty())
            url += $"{{{{={ParentIdProperty}}}}}/";
        if (!IdProperty.IsEmpty())
            url += $"{{{{={IdProperty}}}}}";
        output.Attributes.Add("href", url);
        output.Attributes.Add("title", Title);
        output.Attributes.Add("data-method", Method);
        output.Attributes.Add("class", Class);
        output.Attributes.AddIf("data-confirm", Confirm, !Confirm.IsEmpty());
        output.Attributes.AddIf("data-prompt", Prompt, !Prompt.IsEmpty());

        if (InnerContent != null)
            output.Content.AppendHtml(InnerContent);
        else if (!TextProperty.IsEmpty())
            output.Content.AppendHtml($"{{{{={TextProperty}}}}}");

        base.Process(context, output);
    }
}
