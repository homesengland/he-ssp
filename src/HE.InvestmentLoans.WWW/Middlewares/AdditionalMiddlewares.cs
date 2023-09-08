namespace HE.InvestmentLoans.WWW.Middlewares;

public static class AdditionalMiddlewares
{
    public static IApplicationBuilder ConfigureAdditionalMiddlewares(
        this IApplicationBuilder builder)
    {
        var middlewares = builder.ApplicationServices.GetServices<IMiddleware>();

        foreach (var middleware in middlewares)
        {
            builder.UseMiddleware(middleware.GetType());
        }

        return builder;
    }
}
