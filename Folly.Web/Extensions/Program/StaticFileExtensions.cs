using Microsoft.Extensions.FileProviders;

namespace Folly.Extensions.Program;

public static class StaticFileExtensions {
    public static IApplicationBuilder UseStaticFilesForEnvironment(this IApplicationBuilder app, WebApplicationBuilder builder) {
        if (builder.Environment.IsDevelopment()) {
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Assets")),
                RequestPath = "/Assets"
            });
        } else {
            app.UseStaticFiles();
        }

        return app;
    }
}
