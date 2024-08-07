using HE.UtilsService.Api.Configuration;
using HE.UtilsService.Api.Extensions;
using HE.UtilsService.Api.Middlewares;
using HE.UtilsService.BannerNotification.Configuration;
using HE.UtilsService.SharePoint.Configuration;
using HE.UtilsService.SharePoint.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfiguration"));

builder.Services.AddConfigs();
builder.Services.AddSharePointServices();
builder.Services.AddAutoMapper(typeof(SpAutoMapperProfile));
builder.Services.AddBannerNotificationModule();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();
app.MapHealthChecks("/readyz", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/livez", new HealthCheckOptions { Predicate = _ => false });

app.Run();
