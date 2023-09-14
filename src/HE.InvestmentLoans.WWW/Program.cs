using He.Identity.Auth0;
using He.Identity.Mvc;
using HE.InvestmentLoans.Common.Infrastructure.Middlewares;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.WWW.Config;
using HE.InvestmentLoans.WWW.Extensions;
using HE.InvestmentLoans.WWW.Filters;
using HE.InvestmentLoans.WWW.Middlewares;
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
builder.Services.AddCache(config.Cache);

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();

var mvcBuilder = builder.Services.AddControllersWithViews(config => config.Filters.Add<ExceptionFilter>());

builder.Services.ConfigureHeCookieSettings(
    mvcBuilder,
    configure => configure.WithAspNetCore().WithHeIdentity());

var heIdentityConfiguration = new HeIdentityCookieConfiguration
{
    Domain = config.Auth0.Domain,
    ClientId = config.Auth0.ClientId,
    ClientSecret = config.Auth0.ClientSecret,
    SupportEmail = config.SupportEmail,
};
mvcBuilder.AddHeIdentityCookieAuth(heIdentityConfiguration, builder.Environment);

var auth0Config = new He.Identity.Auth0.Auth0Config(
    config.Auth0.Domain,
    config.Auth0.ClientId,
    config.Auth0.ClientSecret);
var auth0ManagementConfig = new Auth0ManagementConfig(
    config.Auth0.Domain,
    config.Auth0.ClientId,
    config.Auth0.ClientSecret,
    config.Auth0.ManagementClientAudience,
    config.Auth0.UserConnection);

builder.Services.ConfigureIdentityManagementService(x => x.UseAuth0(auth0Config, auth0ManagementConfig));

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
app.UseSession();
app.UseRouting();
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
