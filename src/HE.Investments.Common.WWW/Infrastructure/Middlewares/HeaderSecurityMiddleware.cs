using Microsoft.AspNetCore.Http;

namespace HE.Investments.Common.WWW.Infrastructure.Middlewares;

public class HeaderSecurityMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderSecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, NonceModel nonce)
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        var externalGaScriptUrl = "https://www.googletagmanager.com/gtag/js";
#pragma warning restore S1075 // URIs should not be hardcoded
        var builtInGaScriptNonceHeader = $"nonce-{nonce.GaNonce}";
        var gaConnectAndImgDomains = "*.analytics.google.com *.google-analytics.com";

        // ga domains are required in connect and img src for google analytics to function - see: https://support.google.com/analytics/answer/12017362
        context.Response.Headers.ContentSecurityPolicy =
            $"default-src 'self'; script-src 'self' '{builtInGaScriptNonceHeader}' {externalGaScriptUrl}; connect-src 'self' {gaConnectAndImgDomains}; img-src 'self' {gaConnectAndImgDomains}";
        context.Response.Headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
        context.Response.Headers.XFrameOptions = "DENY";
        context.Response.Headers.XContentTypeOptions = "nosniff";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
        context.Response.Headers.Pragma = "No-cache";
        context.Response.Headers.CacheControl = "No-cache";
        context.Response.Headers.XXSSProtection = "0";
        context.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        context.Response.Headers["Permissions-Policy"] = "interest-cohort=()";
        await _next(context);
    }
}
