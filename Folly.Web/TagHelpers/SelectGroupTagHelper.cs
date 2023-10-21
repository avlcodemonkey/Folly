using System.Text.Encodings.Web;
using Folly.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public sealed class SelectGroupTagHelper : GroupBaseTagHelper {
    private IHtmlContent BuildInput(TagHelperAttributeList attributes) {
        if (string.IsNullOrWhiteSpace(FieldName))
            return HtmlString.Empty;

        var input = new TagBuilder("select");
        // add any attributes passed in first. we'll overwrite ones we need as we build
        attributes.ToList().ForEach(x => input.MergeAttribute(x.Name, x.Value.ToString()));

        input.AddCssClass("form-input");
        input.AddCssClass("form-select");
        input.MergeAttribute("id", FieldName, true);
        input.MergeAttribute("name", FieldName, true);
        input.SetAttributeIf("required", "true", Required == true || (!Required.HasValue && For?.Metadata.IsRequired == true));

        var selectedValue = For?.ModelExplorer.Model?.ToString();
        input.InnerHtml.AppendHtml(new TagBuilder("option"));
        Options.ToList().ForEach(x => {
            var opt = new TagBuilder("option");
            opt.MergeAttribute("value", x.Value.Trim());
            opt.SetAttributeIf("selected", "true", selectedValue == x.Value);
            opt.InnerHtml.Append(x.Text.Trim());
            input.InnerHtml.AppendHtml(opt);
        });

        return input;
    }

    public SelectGroupTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public IEnumerable<SelectListItem> Options { get; set; } = Enumerable.Empty<SelectListItem>();

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        Contextualize();

        if (Options != null)
            Options = Options.Where(x => !x.Value.IsEmpty()).GroupBy(x => x.Value).Select(x => x.First());

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput(output.Attributes));
        inputGroup.InnerHtml.AppendHtml(BuildHelp());

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Clear();
        output.AddClass("mb-1", HtmlEncoder.Default);
        output.Content.AppendHtml(BuildLabel());
        output.Content.AppendHtml(inputGroup);

        await base.ProcessAsync(context, output);
    }
}
