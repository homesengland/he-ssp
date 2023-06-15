using HE.InvestmentLoans.WWW.Extensions;
using HE.InvestmentLoans.WWW.Models;

namespace HE.InvestmentLoans.WWW.Middlewares;

public class HeaderSecurityMiddleware
{
    private readonly RequestDelegate next;

    public HeaderSecurityMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, NonceModel nonce)
    {
        var externalGaScriptUrl = "https://www.googletagmanager.com/gtag/js";
        var builtInGaScriptNonceHeader = $"nonce-{nonce.GaNonce}";
        var gaConnectAndImgDomains = "*.analytics.google.com *.google-analytics.com";
        // ga domains are required in connect and img src for google analytics to function - see: https://support.google.com/analytics/answer/12017362

        context.Response.Headers.AddOrUpdate("Content-Security-Policy", $"default-src 'self'; script-src 'self' '{builtInGaScriptNonceHeader}' {externalGaScriptUrl}; connect-src 'self' {gaConnectAndImgDomains}; img-src 'self' {gaConnectAndImgDomains}");
        context.Response.Headers.AddOrUpdate("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.AddOrUpdate("X-Frame-Options", "DENY");
        context.Response.Headers.AddOrUpdate("X-Content-Type-Options", "nosniff");
        context.Response.Headers.AddOrUpdate("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.AddOrUpdate("X-Permitted-Cross-Domain-Policies", "none");
        context.Response.Headers.AddOrUpdate("Pragma", "No-cache");
        context.Response.Headers.AddOrUpdate("Cache-control", "No-cache");
        context.Response.Headers.AddOrUpdate("X-XSS-Protection", "0");
        context.Response.Headers.AddOrUpdate("X-Robots-Tag", "noindex, nofollow");
        context.Response.Headers.AddOrUpdate("Permissions-Policy", "interest-cohort=()");
        await next(context);
    }
}

public static partial class HeaderSecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderSecurity(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderSecurityMiddleware>();
    }
}
