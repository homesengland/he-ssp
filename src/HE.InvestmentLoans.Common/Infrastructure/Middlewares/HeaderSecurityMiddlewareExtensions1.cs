using Microsoft.AspNetCore.Builder;

namespace HE.InvestmentLoans.Common.Infrastructure.Middlewares;

public static partial class HeaderSecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderSecurity(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderSecurityMiddleware>();
    }
}
