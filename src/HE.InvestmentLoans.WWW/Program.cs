using HE.InvestmentLoans.Common.Authorization;
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
builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.Use((context, next) =>
    {
        // assume all non-development requests are https
        context.Request.Scheme = "https";
        return next();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHeaderSecurity();
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
app.UseMiddleware<HeaderSecurityMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050
