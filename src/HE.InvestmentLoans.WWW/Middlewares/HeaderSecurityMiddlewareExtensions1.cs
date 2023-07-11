namespace HE.InvestmentLoans.WWW.Middlewares;

public static partial class HeaderSecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderSecurity(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderSecurityMiddleware>();
    }
}
