using HE.InvestmentLoans.Common.Infrastructure.Middlewares;
using HE.InvestmentLoans.Common.Models.App;
using HE.Investments.Account.WWW.Config;
using HE.Investments.Account.WWW.Middlewares;
using HE.Investments.Common.CRM;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.Cache;
using HE.Investments.Common.WWW.Partials;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var appConfig = builder.Configuration.GetSection("AppConfiguration").Get<AppConfig>();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
builder.Services.AddCommonBuildingBlocks();
builder.Services.AddCache(appConfig.Cache, appConfig.AppName!);
builder.Services.AddCrmConnection();

var mvcBuilder = builder.Services.AddControllersWithViews();
builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/error");
    app.UsePageNotFound();
    app.UseHsts();
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
