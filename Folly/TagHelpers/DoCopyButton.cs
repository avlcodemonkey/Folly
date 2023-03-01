using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Folly.TagHelpers;

public class DoCopyButtonTagHelper : DoLinkTagHelper
{
    public DoCopyButtonTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper, httpContextAccessor, urlHelperFactory)
    {
        Title = Core.Copy;
        Class = "btn btn-info";
        InnerContent = new TagBuilder("i");
        InnerContent.AddCssClass("pr");
        InnerContent.AddCssClass("pr-clone");
        Action = "Copy";
    }
}
