using Folly.Controllers;
using Folly.Extensions;
using Folly.Extensions.Program;
using Folly.Services;
using Folly.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<IISServerOptions>(options => options.AllowSynchronousIO = true)
    .ConfigureDb()
    .ConfigureHealthChecks()
    .AddScoped<IViewService, ViewService>()
    .AddSession()
    .ConfigureAuthentication(builder.Configuration)
    .AddAntiforgery()
    .AddSingleton(new MemoryCache(new MemoryCacheOptions()))
    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
    .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
    .AddLocalization(x => x.ResourcesPath = "Resources")
    .AddResponseCompression(options => {
        // enable compression only for assets
        options.EnableForHttps = true;
        options.MimeTypes = new List<string>() { "text/css", "application/javascript", "text/javascript", "font/woff2" };
    })
    .AddMvc(options => options.Filters.Add(new RequireHttpsAttribute()))
    .AddRazorRuntimeCompilation()
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

var app = builder.Build();

app
    .UseHealthChecks()
    .UseHttpsRedirection()
    .UseSecurityHeaders()
    .UseStatusCodePagesWithReExecute($"/{nameof(ErrorController).StripController()}", "?code={0}")
    .UseExceptionHandling(builder)
    .UseSession()
    .UseResponseCompression()
    .UseStaticFiles()
    .UseRequestLocalization()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseLocalization()
    .UseMiddleware<VersionCheckMiddleware>();

app.MapControllerRoute("parentChild", "{controller}/{action}/{parentId:int}/{id:int}");
app.MapControllerRoute("default", $"{{controller={nameof(DashboardController).StripController()}}}/{{action={nameof(DashboardController.Index)}}}/{{id:int?}}");

app.Run();
