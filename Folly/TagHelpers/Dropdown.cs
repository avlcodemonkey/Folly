using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class DropdownListItem
{
    public string Action { get; set; }
    public string Confirm { get; set; }
    public string Controller { get; set; }
    public string ExtraClasses { get; set; }
    public Icon Icon { get; set; }
    public string IconExtraClasses { get; set; }
    public string Label { get; set; }
    public HttpVerb Method { get; set; } = HttpVerb.Get;
    public object RouteValues { get; set; }
}

public class DropdownTagHelper : BaseTagHelper
{
    readonly IUrlHelperFactory UrlHelperFactory;

    public DropdownTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : base(htmlHelper)
    {
        UrlHelperFactory = urlHelperFactory;
    }

    public string Id { get; set; }
    public bool IsChecked { get; set; }
    public IEnumerable<DropdownListItem> Items { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string TargetId { get; set; }
    public DataToggle? Toggle { get; set; }
    public string Value { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        var button = new TagBuilder("button");
        button.AddCssClass("btn btn-secondary dropdown-toggle");
        button.Attributes["type"] = "button";
        button.Attributes["data-toggle"] = DataToggle.Dropdown.ToHyphenCase();
        button.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        var ul = new TagBuilder("ul");
        ul.AddCssClass("menu");
        Items.Each(x => {
            var li = new TagBuilder("li");
            li.AddCssClass("menu-item c-hand");
            if (Toggle.HasValue)
            {
                li.Attributes["data-toggle"] = Toggle.ToHyphenCase();
                li.Attributes["data-target"] = $"#{TargetId}";
                li.Attributes["data-value"] = x.Label;
                li.InnerHtml.Append(x.Label);
            }
            else
            {
                var a = new TagBuilder("a");
                var urlHelper = UrlHelperFactory.GetUrlHelper(HtmlHelper.ViewContext);
                a.Attributes.Add("title", x.Label);
                a.Attributes.Add("data-method", x.Method.ToString());
                a.Attributes.AddIf("href", urlHelper.Action(x.Action, x.Controller, x.RouteValues), !x.Controller.IsEmpty());
                a.Attributes.AddIf("data-confirm", x.Confirm, !x.Confirm.IsEmpty());
                if (!x.ExtraClasses.IsEmpty())
                    a.AddCssClass($" {x.ExtraClasses}");
                if (x.Icon.ToString().IsEmpty())
                {
                    a.InnerHtml.Append(x.Label);
                }
                else
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass($"fl fl-{x.Icon.ToCssClass()}");
                    if (!x.IconExtraClasses.IsEmpty())
                        i.AddCssClass($" {x.IconExtraClasses}");
                    a.InnerHtml.AppendHtml(i);
                    a.InnerHtml.Append($" {x.Label}");
                }
                li.InnerHtml.AppendHtml(a);
            }
            ul.InnerHtml.AppendHtml(li);
        });

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        output.AddClass("dropdown", HtmlEncoder.Default);
        output.AddClass("dropdown-right", HtmlEncoder.Default);
        output.AddClass("text-left", HtmlEncoder.Default);
        output.Content.AppendHtml(button);
        output.Content.AppendHtml(ul);

        base.Process(context, output);
    }
}
