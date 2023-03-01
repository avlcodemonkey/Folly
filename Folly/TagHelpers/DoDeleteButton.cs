using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Folly.TagHelpers;

public class DoDeleteButtonTagHelper : DoLinkTagHelper
{
    public DoDeleteButtonTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper, httpContextAccessor, urlHelperFactory)
    {
        Title = Core.Delete;
        Method = "DELETE";
        Class = "btn btn-error";
        InnerContent = new TagBuilder("i");
        InnerContent.AddCssClass("pr");
        InnerContent.AddCssClass("pr-trash");
        Action = "Delete";
    }
}
