using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class DoTableTagHelper : BaseTagHelper
{
    public bool CheckUpdateDate { get; set; }
    public string DisplayCurrencyFormat { get; set; }
    public string DisplayDateFormat { get; set; }
    public string DisplayTimeFormat { get; set; }
    public bool Editable { get; set; } = true;
    public string Id { get; set; }
    public int ItemsPerPage { get; set; } = 10;
    public bool LoadAll { get; set; } = true;
    public HttpVerb RequestMethod { get; set; } = HttpVerb.Post;
    public object RequestParams { get; set; }
    public string RequestUrl { get; set; }
    public bool Searchable { get; set; } = true;
    public HttpVerb? StoreRequestMethod { get; set; }
    public string StoreUrl { get; set; }
    public string Template { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var table = new TagBuilder("div");
        if (!Searchable)
            table.AddCssClass("no-autofocus");
        table.Attributes.Add("id", Id);
        table.Attributes.Add("data-toggle", DataToggle.DoTable.ToHyphenCase());
        table.Attributes.Add("data-template", Template);
        table.Attributes.Add("data-request-url", RequestUrl);
        table.Attributes.Add("data-items-per-page", ItemsPerPage.ToString());
        table.Attributes.Add("data-editable", Editable.ToString());
        table.Attributes.Add("data-searchable", Searchable.ToString());
        table.Attributes.Add("data-load-all", LoadAll.ToString());
        table.Attributes.Add("data-request-method", RequestMethod.ToString());
        table.Attributes.Add("data-check-update-date", CheckUpdateDate.ToString());
        table.Attributes.AddIf("data-store-url", StoreUrl, !StoreUrl.IsEmpty());
        table.Attributes.AddIf("data-store-request-method", StoreRequestMethod.ToString(), StoreRequestMethod.HasValue);
        table.Attributes.AddIf("data-display-date-format", DisplayDateFormat, !DisplayDateFormat.IsEmpty());
        table.Attributes.AddIf("data-display-time-format", DisplayTimeFormat, !DisplayTimeFormat.IsEmpty());
        table.Attributes.AddIf("data-display-currency-format", DisplayCurrencyFormat, !DisplayCurrencyFormat.IsEmpty());
        //table.Attributes.AddIf("json-request-params", JsonSerializer.ToJsonString(RequestParams), RequestParams != null);

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        output.AddClass("col-12", HtmlEncoder.Default);
        output.Content.AppendHtml(table);
        base.Process(context, output);
    }
}
