using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class GroupBaseTagHelper : BaseTagHelper {
    public GroupBaseTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public string? FieldName => For?.Name ?? Name;
    public string? FieldTitle => For?.Metadata.DisplayName ?? Title;
    public ModelExpression? For { get; set; }
    public string? HelpText { get; set; }
    public bool? Required { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }

    public static TagBuilder BuildInputGroup() {
        var inputGroup = new TagBuilder("div");
        inputGroup.AddCssClass("input-group");
        return inputGroup;
    }

    public static int GetMaxLength(IReadOnlyList<object> validatorMetadata) {
        for (var i = 0; i < validatorMetadata.Count; i++) {
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MaximumLength > 0)
                return stringLengthAttribute.MaximumLength;
            if (validatorMetadata[i] is MaxLengthAttribute maxLengthAttribute && maxLengthAttribute.Length > 0)
                return maxLengthAttribute.Length;
        }
        return 0;
    }

    public static int GetMinLength(IReadOnlyList<object> validatorMetadata) {
        for (var i = 0; i < validatorMetadata.Count; i++) {
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MinimumLength > 0)
                return stringLengthAttribute.MinimumLength;
            if (validatorMetadata[i] is MinLengthAttribute minLengthAttribute && minLengthAttribute.Length > 0)
                return minLengthAttribute.Length;
        }
        return 0;
    }

    public IHtmlContent BuildHelp() {
        if (HtmlHelper!.ViewContext.HttpContext?.WantsHelp() != true)
            return HtmlString.Empty;

        if (HelpText.IsEmpty() && For != null)
            HelpText = ContextHelp.ResourceManager.GetString($"{For.Metadata.ContainerType!.Name}_{For.Metadata.PropertyName}", CultureInfo.InvariantCulture) ?? "";
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
        button.MergeAttribute("hx-alert-content", HelpText!.Replace("\"", "&quot;"));
        button.InnerHtml.AppendHtml(icon);

        var span = new TagBuilder("span");
        span.AddCssClass("input-group-addon");
        span.InnerHtml.AppendHtml(button);

        return span;
    }

    public IHtmlContent BuildLabel(string? forField = null) {
        if (string.IsNullOrWhiteSpace(FieldTitle))
            return HtmlString.Empty;

        var label = new TagBuilder("label");
        if (!string.IsNullOrWhiteSpace(forField ?? FieldName))
            label.Attributes.Add("for", forField ?? FieldName);
        label.InnerHtml.Append(FieldTitle);
        return label;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) => await base.ProcessAsync(context, output);
}
