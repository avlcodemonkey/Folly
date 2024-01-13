using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Folly.Extensions;
using Folly.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

/// <summary>
/// Provides helpers for creating input groups.  Should not be used directly.
/// </summary>
/// <param name="htmlHelper">HtmlHelper for rendering.</param>
public class GroupBaseTagHelper(IHtmlHelper htmlHelper) : BaseTagHelper(htmlHelper) {
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

    public async Task<IHtmlContent> BuildHelp() {
        if (HtmlHelper!.ViewContext.HttpContext?.WantsHelp() != true) {
            return HtmlString.Empty;
        }

        if (string.IsNullOrWhiteSpace(HelpText) && For != null) {
            HelpText = ContextHelp.ResourceManager.GetString($"{For.Metadata.ContainerType!.Name}_{For.Metadata.PropertyName}", CultureInfo.InvariantCulture) ?? "";
        }

        if (string.IsNullOrWhiteSpace(HelpText)) {
            return HtmlString.Empty;
        }

        var button = new TagBuilder("button");
        button.AddCssClass("button success button-icon");
        button.MergeAttribute("type", "button");
        button.MergeAttribute("role", "button");
        button.MergeAttribute("data-dialog-content", HelpText!.Replace("\"", "&quot;"));
        button.MergeAttribute("data-dialog-ok", Core.Okay);
        button.InnerHtml.AppendHtml(await HtmlHelper!.PartialAsync("Icons/_Flag"));

        var labelSpan = new TagBuilder("span");
        labelSpan.InnerHtml.Append(Core.Help);
        labelSpan.AddCssClass("visually-hidden");
        button.InnerHtml.AppendHtml(labelSpan);

        var dialog = new TagBuilder("nilla-info");
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
