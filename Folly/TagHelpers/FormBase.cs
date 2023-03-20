using System.ComponentModel.DataAnnotations;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class FormBaseTagHelper : BaseTagHelper
{
    public FormBaseTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public bool? Disabled { get; set; }
    public string FieldName => For == null ? Name : For.Name;
    public string FieldTitle => For == null ? Title : For.Metadata.DisplayName;
    public ModelExpression For { get; set; }
    public string HelpText { get; set; }
    public bool? IsRequired { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }

    public static TagBuilder BuildInputGroup()
    {
        var inputGroup = new TagBuilder("div");
        inputGroup.AddCssClass("input-group");
        return inputGroup;
    }

    public static int GetMaxLength(IReadOnlyList<object> validatorMetadata)
    {
        for (var i = 0; i < validatorMetadata.Count; i++)
        {
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MaximumLength > 0)
                return stringLengthAttribute.MaximumLength;
            if (validatorMetadata[i] is MaxLengthAttribute maxLengthAttribute && maxLengthAttribute.Length > 0)
                return maxLengthAttribute.Length;
        }
        return 0;
    }

    public static int GetMinLength(IReadOnlyList<object> validatorMetadata)
    {
        for (var i = 0; i < validatorMetadata.Count; i++)
        {
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MinimumLength > 0)
                return stringLengthAttribute.MinimumLength;
            if (validatorMetadata[i] is MinLengthAttribute minLengthAttribute && minLengthAttribute.Length > 0)
                return minLengthAttribute.Length;
        }
        return 0;
    }

    public IHtmlContent BuildHelp()
    {
        if (HtmlHelper.ViewContext.HttpContext?.WantsHelp() != true)
            return HtmlString.Empty;

        if (HelpText.IsEmpty() && For != null)
            HelpText = ContextHelp.ResourceManager.GetString($"{For.Metadata.ContainerType.Name}_{For.Metadata.PropertyName}") ?? "";
        if (HelpText.IsEmpty())
            return HtmlString.Empty;

        var icon = new TagBuilder("i");
        icon.AddCssClass("fl");
        icon.AddCssClass("fl-help");

        var button = new TagBuilder("button");
        button.AddCssClass("button secondary icon-only");
        button.MergeAttribute("type", "button");
        button.MergeAttribute("role", "button");
        button.MergeAttribute("hx-get", "#");
        button.MergeAttribute("hx-alert-content", HelpText.Replace("\"", "&quot;"));
        button.InnerHtml.AppendHtml(icon);

        var span = new TagBuilder("span");
        span.AddCssClass("input-group-addon");
        span.InnerHtml.AppendHtml(button);

        return span;
    }

    public IHtmlContent BuildLabel(string forField = null)
    {
        var label = new TagBuilder("label");
        label.Attributes.Add("for", forField ?? FieldName);
        label.InnerHtml.Append(FieldTitle);
        return label;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) => base.Process(context, output);
}
