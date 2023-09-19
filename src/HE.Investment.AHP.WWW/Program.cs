using HE.Investment.AHP.WWW.Config;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Infrastructure.ErrorHandling;
using HE.InvestmentLoans.Common.Infrastructure.Middlewares;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHttpClient();
builder.Services.AddWebModule();
builder.Services.AddFeatureManagement();
var mvcBuilder = builder.Services.AddControllersWithViews();
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

app.Run();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050
