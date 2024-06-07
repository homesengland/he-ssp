using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks;

public static class EndpointRouteBuilderExtensions
{
    public static void MapHeHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/readyz", new HealthCheckOptions { ResponseWriter = ReadyResponseWriter });
        endpoints.MapHealthChecks("/livez", new HealthCheckOptions { Predicate = _ => false });
    }

    private static async Task ReadyResponseWriter(HttpContext context, HealthReport report)
    {
        if (!context.Request.Query.ContainsKey("verbose"))
        {
            await HealthCheckResponseWriters.WriteMinimalPlaintextResponse(context, report);
        }
        else
        {
            await HealthCheckResponseWriters.WriteExtendedJsonResponse(context, report);
        }
    }
}
