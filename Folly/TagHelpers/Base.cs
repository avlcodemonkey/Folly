﻿using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class BaseTagHelper : TagHelper
{
    public BaseTagHelper() { }

    public BaseTagHelper(IHtmlHelper htmlHelper) => HtmlHelper = htmlHelper;

    [HtmlAttributeNotBound]
    public IHtmlHelper? HtmlHelper { get; set; }
    public bool NoRender { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public static void UseInputGroup(TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        output.AddClass("form-group", HtmlEncoder.Default);
    }

    public void Contextualize()
    {
        if (HtmlHelper != null && ViewContext != null)
            (HtmlHelper as IViewContextAware)!.Contextualize(ViewContext);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (NoRender)
            output.SuppressOutput();
        base.Process(context, output);
    }
}
