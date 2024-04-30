using HE.Investments.Account.WWW.Config;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.Cache;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.Common.WWW.Partials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var appConfig = builder.Configuration.GetSection("AppConfiguration").Get<AppConfig>();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
builder.Services.AddCommonBuildingBlocks();
builder.Services.AddCache(appConfig!.Cache);
builder.Services.AddCrmConnection();

var mvcBuilder = builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    options.Filters.Add<ExceptionFilterAttribute>();
});
builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    var errorViewPaths = app.Services.GetRequiredService<IErrorViewPaths>();
    app.UseExceptionHandler(errorViewPaths.ErrorRoute)
        .UsePageNotFound(errorViewPaths.PageNotFoundRoute)
        .UseHttps();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<HeaderSecurityMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#pragma warning disable CA1050
namespace HE.Investments.Account.WWW
{
    public partial class Program
    {
    }
}
#pragma warning restore CA1050
