namespace HE.Investments.Account.WWW.Middlewares;

public static class OrganisationMiddlewareExtensions
{
    public static IApplicationBuilder UsePageNotFound(this IApplicationBuilder app)
    {
        return app.UseMiddleware<PageNotFoundMiddleware>();
    }
}
