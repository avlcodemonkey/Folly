using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Folly.TagHelpers;

public class LitAlertTagHelper : BaseTagHelper
{
    public LitAlertTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    public LitAlertType Type { get; set; } = LitAlertType.Success;

    public LitLanguage? Language { get; set; }

    public bool NoDismiss { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Contextualize();

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "lit-alert";
        output.Attributes.Add("type", Type.ToString().ToLower());
        output.Attributes.AddIf("no-dismiss", "true", NoDismiss);

        var culture = HtmlHelper.ViewContext.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture;
        var lang = ((Language != null ? Language.ToString() : culture?.TwoLetterISOLanguageName) ?? "en").ToLower();
        output.Attributes.Add("lang", lang);

        base.Process(context, output);
    }
}
