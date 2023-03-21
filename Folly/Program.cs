using System.Globalization;
using System.Text.Json;
using Folly;
using Folly.Configuration;
using Folly.Controllers;
using Folly.Domain.Models;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfiguration();
builder.Configuration.Bind("App", appConfig);
builder.Services.AddSingleton((IAppConfiguration)appConfig);

builder.Services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

// @todo move out into separate class
builder.Services.AddDbContext<FollyDbContext>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IViewService, ViewService>();

builder.Services.AddSession();

builder.Services.ConfigureAuthentication(appConfig);

builder.Services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
builder.Services.AddAuthorization(options => {
    options.AddPolicy(PermissionRequirementHandler.PolicyName, policy => policy.Requirements.Add(new PermissionRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

builder.Services.AddDataProtection();
builder.Services.AddAntiforgery();

builder.Services.AddSingleton(new MemoryCache(new MemoryCacheOptions()));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddLocalization(x => { x.ResourcesPath = "Resources"; });

// enable compression only for assets
builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
    options.MimeTypes = new List<string>() { "text/css", "application/javascript", "text/javascript", "font/woff2" };
});

// @todo break out into separate class?
builder.Services.AddMvc(options => {
    options.Filters.Add(new RequireHttpsAttribute());
})
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(configure => {
        configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var app = builder.Build();

// force all requests to https
app.UseHttpsRedirection();

// harden headers
app.UseSecureHeaders();

app.UseStatusCodePagesWithReExecute($"/{nameof(ErrorController).StripController()}", "?code={0}");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(builder => {
        builder.Run(async context => {
            var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (error != null)
                try
                {
                    // @todo log error
                    //Serilog.Log.Error(error.Error, error.Error.Message);
                }
                catch { }

            if (context.Request.ContentType?.ToLower()?.Contains("json") == true)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                using var writer = new StreamWriter(context.Response.Body);
                writer.Write(JsonSerializer.Serialize(new { Error = "An unexpected error occurred." }));
                await writer.FlushAsync().ConfigureAwait(false);
            }
            else
            {
                context.Response.Redirect($"/{nameof(ErrorController).StripController()}/{nameof(ErrorController.Index)}");
            }
        });
    });
}

app.UseSession();

app.UseResponseCompression();

app.UseStaticFiles();

app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

var cultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("es") };
var localizationOptions = new RequestLocalizationOptions {
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
};
localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider() { });
app.UseRequestLocalization(localizationOptions);

app.MapControllerRoute("parentChild", "{controller}/{action}/{parentId:int}/{id:int}");
app.MapControllerRoute("default", $"{{controller={nameof(DashboardController).StripController()}}}/{{action={nameof(DashboardController.Index)}}}/{{id:int?}}");

app.Run();
