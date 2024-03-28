using System.Text.Encodings.Web;
using Folly.Constants;
using Folly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class FormContentTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory) : BaseTagHelper(htmlHelper) {
    private readonly IUrlHelperFactory _UrlHelperFactory = urlHelperFactory;

    public string Action { get; set; } = "";
    public string Controller { get; set; } = "";
    public object? For { get; set; }
    public HttpMethod Method { get; set; } = HttpMethod.Post;
    public object? RouteValues { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        // @todo might be worth using this same pattern for the hidden id input
        TagBuilder? rowVersionInput = null;
        if (For != null) {
            var type = For.GetType();
            Controller = type.Name;
            Method = ((BaseModel)For).IsCreate ? HttpMethod.Post : HttpMethod.Put;

            // if the model is versioned create the hidden input for the rowversion field
            if (type.IsAssignableTo(typeof(VersionedModel))) {
                rowVersionInput = new TagBuilder("input");
                rowVersionInput.MergeAttribute("type", "hidden");
                rowVersionInput.MergeAttribute("name", nameof(VersionedModel.RowVersion));
                rowVersionInput.MergeAttribute("value", ((VersionedModel)For).RowVersion.ToString());
            }
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "form";
        output.AddClass("container", HtmlEncoder.Default);

        if (Method == HttpMethod.Get || Method == HttpMethod.Post) {
            // GET and POST can use standard method attribute
            output.Attributes.SetAttribute("method", Method.ToString());
        } else {
            // other methods have to use custom attribute
            output.Attributes.SetAttribute(PJax.MethodAttribute, Method.ToString());
        }

        output.Attributes.SetAttribute("id", $"{Action}{Controller}Form");

        var urlHelper = _UrlHelperFactory.GetUrlHelper(HtmlHelper!.ViewContext);
        output.Attributes.SetAttribute("action", urlHelper.Action(Action, Controller, RouteValues));

        if (rowVersionInput != null) {
            // add the RowVersion hidden input when needed
            output.Content.AppendHtml(rowVersionInput);
        }

        output.Content.AppendHtml(HtmlHelper.AntiForgeryToken());
        output.Content.AppendHtml(output.GetChildContentAsync().Result);

        await base.ProcessAsync(context, output);
    }
}
