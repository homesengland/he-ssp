using HE.Investment.AHP.WWW.Config;
using HE.InvestmentLoans.Common.Infrastructure.Middlewares;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Partials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();
builder.Services.AddWebModule(builder);
builder.Services.AddFeatureManagement();
builder.Services.AddCommonBuildingBlocks();
var mvcBuilder = builder.Services
    .AddControllersWithViews(options =>
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
    .AddMvcOptions(options =>
        options.Filters.Add(
            new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None }));

builder.AddIdentityProviderConfiguration(mvcBuilder);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
app.MapControllerRoute(
    name: "section",
    pattern: "application/{applicationId}/{controller}/{action}");
app.MapControllerRoute(
    name: "subSection",
    pattern: "application/{applicationId}/{controller}/{id?}/{action}");

app.MapControllerRoute(
    name: "action",
    pattern: "{controller}/{id}/{action}");

app.Run();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050
