using HE.Investment.AHP.WWW.Config;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.Cache;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.HealthChecks;
using HE.Investments.Common.WWW.Infrastructure.HealthChecks.Connectors;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.Common.WWW.Partials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var appConfig = builder.Configuration.GetSection("AppConfiguration").Get<AppConfig>();

builder.Services.AddCache(appConfig!.Cache);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();
builder.Services.AddCrmConnection();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
builder.Services.AddCommonBuildingBlocks();
builder.Services.AddHeHealthChecks(typeof(RedisHealthCheckConnector), typeof(CrmHealthCheckConnector));

var mvcBuilder = builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
        options.Filters.Add<ExceptionFilterAttribute>();
    })
    .AddMvcOptions(options =>
        options.Filters.Add(
            new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None }));

builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();
const string globalRoutePrefix = "/ahp";
app.UsePathBase(globalRoutePrefix);

if (!app.Environment.IsDevelopment())
{
    var errorViewPaths = app.Services.GetRequiredService<IErrorViewPaths>();
    app.UseExceptionHandler(errorViewPaths.ErrorRoute)
        .UsePageNotFound(errorViewPaths.PageNotFoundRoute)
        .UseHttps();
}

app.UseCustomDisableRequestLimitSize("/design-plans", "/upload-design-plans-file", "/stakeholder-discussions");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<HeaderSecurityMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHeHealthChecks();

app.Run();

#pragma warning disable CA1050
namespace HE.Investment.AHP.WWW
{
    public partial class Program
    {
    }
}
#pragma warning restore CA1050
