using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Infrastructure.ErrorHandling;
using HE.InvestmentLoans.Common.Infrastructure.Middlewares;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.WWW.Config;
using HE.InvestmentLoans.WWW.Extensions;
using HE.InvestmentLoans.WWW.Middlewares;
using He.Investments.AspNetCore.UI.Common;
using HE.Investments.DocumentService.Extensions;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;
using Microsoft.FeatureManagement;

#pragma warning disable CA1852
#pragma warning disable CA1812
var builder = WebApplication.CreateBuilder(args);
#pragma warning restore CA1812
#pragma warning restore CA1852

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfiguration"));

builder.Services.AddConfigs();

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var config = builder.Services.BuildServiceProvider().GetRequiredService<IAppConfig>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

var sessionCookieName = ".AspNetCore.Session";
builder.Services.AddSession(options =>
{
    options.Cookie.Name = sessionCookieName;
    options.IdleTimeout = TimeSpan.FromMinutes(config.Cache.SessionExpireMinutes);
});
builder.Services.AddCache(config);

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
builder.Services.AddDocumentServiceModule();

var mvcBuilder = builder.Services.AddControllersWithViews(x => x.Filters.Add<ExceptionFilter>());
builder.AddIdentityProviderConfiguration(mvcBuilder);

var assembly = typeof(AssemblyMarkup).Assembly;
builder.Services.AddControllersWithViews()
    .AddApplicationPart(assembly)
    .AddRazorRuntimeCompilation();
builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(
    options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Items["originalPath"] = context.Request.Path.Value;
            context.Items["backUrl"] = context.Request.Headers["Referer"];
            context.Request.Path = "/Home/PageNotFound";
            await next();
        }
    });

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
    app.Use((context, next) =>
    {
        // assume all non-development requests are https
        context.Request.Scheme = "https";
        return next();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<HeaderSecurityMiddleware>();
app.UseCrossSiteScriptingSecurity();
app.ConfigureAdditionalMiddlewares();

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.Strict,
    });
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050
