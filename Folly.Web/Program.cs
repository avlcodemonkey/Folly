using Folly.Configuration;
using Folly.Controllers;
using Folly.Extensions;
using Folly.Extensions.Program;
using Folly.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfiguration();
builder.Configuration.Bind("App", appConfig);

builder.Services
    .AddSingleton((IAppConfiguration)appConfig)
    .Configure<IISServerOptions>(options => options.AllowSynchronousIO = true)
    .ConfigureDb()
    .ConfigureHealthChecks()
    .AddScoped<IViewService, ViewService>()
    .AddSession()
    .ConfigureAuthentication(appConfig)
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
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

var app = builder.Build();

app
    .UseHealthChecks()
    .UseHttpsRedirection()
    .UseSecureHeaders()
    .UseStatusCodePagesWithReExecute($"/{nameof(ErrorController).StripController()}", "?code={0}")
    .UseExceptionHandling(builder)
    .UseSession()
    .UseResponseCompression()
    .UseStaticFiles()
    .UseRequestLocalization()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseLocalization();

app.MapControllerRoute("parentChild", "{controller}/{action}/{parentId:int}/{id:int}");
app.MapControllerRoute("default", $"{{controller={nameof(DashboardController).StripController()}}}/{{action={nameof(DashboardController.Index)}}}/{{id:int?}}");

app.Run();
