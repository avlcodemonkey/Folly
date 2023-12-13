using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Folly.Extensions;
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
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MaximumLength > 0) {
                return stringLengthAttribute.MaximumLength;
            }

            if (validatorMetadata[i] is MaxLengthAttribute maxLengthAttribute && maxLengthAttribute.Length > 0) {
                return maxLengthAttribute.Length;
            }
        }
        return 0;
    }

    public static int GetMinLength(IReadOnlyList<object> validatorMetadata) {
        for (var i = 0; i < validatorMetadata.Count; i++) {
            if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MinimumLength > 0) {
                return stringLengthAttribute.MinimumLength;
            }

            if (validatorMetadata[i] is MinLengthAttribute minLengthAttribute && minLengthAttribute.Length > 0) {
                return minLengthAttribute.Length;
            }
        }
        return 0;
    }

    public IHtmlContent BuildHelp() {
        if (HtmlHelper!.ViewContext.HttpContext?.WantsHelp() != true) {
            return HtmlString.Empty;
        }

        if (string.IsNullOrWhiteSpace(HelpText) && For != null) {
            HelpText = ContextHelp.ResourceManager.GetString($"{For.Metadata.ContainerType!.Name}_{For.Metadata.PropertyName}", CultureInfo.InvariantCulture) ?? "";
        }

        if (string.IsNullOrWhiteSpace(HelpText)) {
            return HtmlString.Empty;
        }

        var icon = new TagBuilder("i");
        icon.AddCssClass("mi");
        icon.AddCssClass("mi-flag");

        var button = new TagBuilder("button");
        button.AddCssClass("button success icon-only");
        button.MergeAttribute("type", "button");
        button.MergeAttribute("role", "button");
        button.InnerHtml.AppendHtml(icon);

        var dialog = new TagBuilder("fw-info-dialog");
        dialog.MergeAttribute("data-content", HelpText!.Replace("\"", "&quot;"));
        dialog.MergeAttribute("data-ok", Core.Okay);
        dialog.InnerHtml.AppendHtml(button);

        var span = new TagBuilder("span");
        span.AddCssClass("input-group-addon");
        span.InnerHtml.AppendHtml(dialog);

        return span;
    }

    public IHtmlContent BuildLabel(string? forField = null) {
        if (string.IsNullOrWhiteSpace(FieldTitle)) {
            return HtmlString.Empty;
        }

        var label = new TagBuilder("label");
        if (!string.IsNullOrWhiteSpace(forField ?? FieldName)) {
            label.MergeAttribute("for", forField ?? FieldName);
        }
        label.InnerHtml.Append(FieldTitle);

        return label;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) => await base.ProcessAsync(context, output);
}
