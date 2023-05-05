using HardHat;

namespace Folly.Utils;

public static class Headers {
    /// <summary>
    /// Harden headers to help secure app.
    /// Using HardHat (https://github.com/TerribleDev/HardHat) even though its been abandoned because it works.
    /// </summary>
    public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder app) {
        app.UseDnsPrefetch(false);
        app.UseHsts(900, true, false);
        app.UseReferrerPolicy(ReferrerPolicy.NoReferrer);
        app.UseIENoOpen();
        app.UseNoMimeSniff();
        app.UseCrossSiteScriptingFilters();
        app.UseContentSecurityPolicy(new ContentSecurityPolicyBuilder()
            .WithDefaultSource(CSPConstants.Self)
            //.WithConnectSource("*", "'self'", "'unsafe-inline'", "data:", "blob:") // added to try to get hot reload to work
            .WithImageSource("'self'", "data:") // allow images from self, including base64 encoding images aka icon fonts
            .WithStyleSource("'self'", "'unsafe-inline'") // allow styles from self and inline from js
            .WithFontSource(CSPConstants.Self, "data:")
            .WithFrameAncestors(CSPConstants.None)
            .WithScriptSource("'self'", "'unsafe-eval'")
            .BuildPolicy()
        );

        return app;
    }
}
