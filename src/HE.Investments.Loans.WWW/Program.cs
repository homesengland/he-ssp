using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.Cache;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.Common.WWW.Partials;
using HE.Investments.DocumentService.Extensions;
using HE.Investments.Loans.Common.Infrastructure.Middlewares;
using HE.Investments.Loans.Common.Models.App;
using HE.Investments.Loans.WWW.Config;
using HE.Investments.Loans.WWW.Extensions;
using HE.Investments.Loans.WWW.Middlewares;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddCache(config.Cache, config.AppName!);
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
builder.Services.AddDocumentServiceModule();
builder.Services.AddCommonBuildingBlocks();

var mvcBuilder = builder.Services.AddControllersWithViews(x => x.Filters.Add<ExceptionFilterAttribute>());
builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();
const string globalRoutePrefix = "/loans";
app.UsePathBase(globalRoutePrefix);

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
app.UseCustomDisableRequestLimitSize("/more-information-about-organization-upload-file", "/more-information-about-organization");

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
namespace HE.Investments.Loans.WWW
{
    public partial class Program
    {
    }
}
#pragma warning restore CA1050
