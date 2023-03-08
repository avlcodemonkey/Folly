﻿using Folly.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class BreadcrumbTagHelper : BaseTagHelper
{
    public BreadcrumbTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        var li = new TagBuilder("li");
        li.AddCssClass("breadcrumb-item");
        li.InnerHtml.AppendHtml(HtmlHelper.ActionLink(Core.Dashboard, "Index", "Dashboard"));

        output.TagName = "ul";
        output.Attributes.Add("id", "breadcrumb");
        output.Content.AppendHtml(li);
        output.Content.AppendHtml(output.GetChildContentAsync().Result);
        base.Process(context, output);
    }
}
