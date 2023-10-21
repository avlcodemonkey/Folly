using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Folly.Extensions.Program;

public static class LocalizationExtensions {
    private static readonly List<CultureInfo> Cultures = new() { new CultureInfo("en"), new CultureInfo("es") };

    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app) {
        var localizationOptions = new RequestLocalizationOptions {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = Cultures,
            SupportedUICultures = Cultures,
        };
        localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider() { });
        app.UseRequestLocalization(localizationOptions);

        return app;
    }
}
