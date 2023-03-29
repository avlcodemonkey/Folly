using Folly.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class FormGroupSelectTagHelper : FormBaseTagHelper
{
    IHtmlContent BuildInput()
    {
        var input = new TagBuilder("select");
        input.AddCssClass("form-input");
        input.AddCssClass("form-select");
        input.Attributes.Add("id", FieldName);
        input.Attributes.Add("name", FieldName);
        input.Attributes.AddIf("required", "true", IsRequired == true || (!IsRequired.HasValue && For?.Metadata.IsRequired == true));
        input.Attributes.AddIf("autofocus", "true", Autofocus);
        input.Attributes.AddIf("disabled", "true", Disabled == true);

        var selectedValue = For?.ModelExplorer.Model?.ToString();
        input.InnerHtml.AppendHtml(new TagBuilder("option"));
        Options.ToList().ForEach(x => {
            var opt = new TagBuilder("option");
            opt.Attributes.Add("value", x.Value.Trim());
            opt.Attributes.AddIf("selected", "true", selectedValue == x.Value);
            opt.InnerHtml.Append(x.Text.Trim());
            input.InnerHtml.AppendHtml(opt);
        });

        return input;
    }

    public FormGroupSelectTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool Autofocus { get; set; }
    public string Match { get; set; }
    public IEnumerable<SelectListItem> Options { get; set; }
    public string Params { get; set; }
    public string Target { get; set; }
    public DataToggle? Toggle { get; set; }
    public string Url { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        if (Options != null)
            Options = Options.Where(x => !x.Value.IsEmpty()).GroupBy(x => x.Value).Select(x => x.First());

        var div = new TagBuilder("div");
        div.AddCssClass("mb-1");
        div.InnerHtml.AppendHtml(BuildLabel());

        var inputGroup = BuildInputGroup();
        inputGroup.InnerHtml.AppendHtml(BuildInput());
        inputGroup.InnerHtml.AppendHtml(BuildHelp());
        div.InnerHtml.AppendHtml(inputGroup);

        output.Content.AppendHtml(div);

        base.Process(context, output);
    }
}
