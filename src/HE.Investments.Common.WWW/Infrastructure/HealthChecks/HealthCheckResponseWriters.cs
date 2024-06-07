using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks;

internal static class HealthCheckResponseWriters
{
    private static readonly byte[] DegradedBytes = Encoding.UTF8.GetBytes(HealthStatus.Degraded.ToString());
    private static readonly byte[] HealthyBytes = Encoding.UTF8.GetBytes(HealthStatus.Healthy.ToString());
    private static readonly byte[] UnhealthyBytes = Encoding.UTF8.GetBytes(HealthStatus.Unhealthy.ToString());

    public static Task WriteMinimalPlaintextResponse(HttpContext httpContext, HealthReport result)
    {
        httpContext.Response.ContentType = "text/plain";
        return result.Status switch
        {
            HealthStatus.Degraded => httpContext.Response.Body.WriteAsync(DegradedBytes.AsMemory()).AsTask(),
            HealthStatus.Healthy => httpContext.Response.Body.WriteAsync(HealthyBytes.AsMemory()).AsTask(),
            HealthStatus.Unhealthy => httpContext.Response.Body.WriteAsync(UnhealthyBytes.AsMemory()).AsTask(),
            _ => httpContext.Response.WriteAsync(result.Status.ToString()),
        };
    }

    public static async Task WriteExtendedJsonResponse(HttpContext httpContext, HealthReport result)
    {
        var response = JsonConvert.SerializeObject(
            new { Status = result.Status.ToString(), Duration = result.TotalDuration, Info = result.Entries.Select(CreateHealthCheckDetails).ToList() },
            Formatting.Indented,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsync(response);
    }

    private static object CreateHealthCheckDetails(KeyValuePair<string, HealthReportEntry> report)
    {
        return new
        {
            report.Key,
            report.Value.Description,
            report.Value.Duration,
            Status = Enum.GetName(typeof(HealthStatus), report.Value.Status),
            Error = report.Value.Exception?.Message,
        };
    }
}
