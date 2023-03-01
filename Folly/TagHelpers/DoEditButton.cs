using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Folly.TagHelpers;

public class DoEditButtonTagHelper : DoLinkTagHelper
{
    public DoEditButtonTagHelper(IHtmlHelper htmlHelper, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory) : base(htmlHelper, httpContextAccessor, urlHelperFactory)
    {
        Title = Core.Edit;
        Class = "btn btn-warning";
        InnerContent = new TagBuilder("i");
        InnerContent.AddCssClass("pr");
        InnerContent.AddCssClass("pr-edit");
        Action = "Edit";
    }
}
