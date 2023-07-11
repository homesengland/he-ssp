﻿namespace HE.InvestmentLoans.WWW.Middlewares;

public static partial class HeaderSecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseCrossSiteScriptingSecurity(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CrossSiteScriptingMiddleware>();
    }
}
