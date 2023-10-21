using System.Globalization;
using System.Text.Json;
using Folly.Controllers;

namespace Folly.Extensions.Program;

public static class ExceptionHandlingExtensions {
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, WebApplicationBuilder builder) {
        if (builder.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler(builder => builder.Run(async context => {
                var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                if (error != null) {
                    try {
                        // @todo log error
                    } catch { }
                }

                if (context.Request.ContentType?.ToLower(CultureInfo.InvariantCulture)?.Contains("json") == true) {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    using var writer = new StreamWriter(context.Response.Body);
                    writer.Write(JsonSerializer.Serialize(new { Error = "An unexpected error occurred." }));
                    await writer.FlushAsync().ConfigureAwait(false);
                } else {
                    context.Response.Redirect($"/{nameof(ErrorController).StripController()}/{nameof(ErrorController.Index)}");
                }
            }));
        }

        return app;
    }
}
