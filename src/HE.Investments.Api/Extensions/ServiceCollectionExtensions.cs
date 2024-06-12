using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConditionalApiContext<TContext, TCrmContext, TApiContext, TContextSelector>(this IServiceCollection services)
        where TContext : class
        where TCrmContext : class, TContext
        where TApiContext : ApiHttpClientBase, TContext
        where TContextSelector : class, TContext
    {
        services.AddScoped<TCrmContext>();
        services.AddHttpClient<TApiContext>().ConfigureApiHttpClient();
        services.AddScoped<Func<TCrmContext>>(x => x.GetRequiredService<TCrmContext>);
        services.AddScoped<Func<TApiContext>>(x => x.GetRequiredService<TApiContext>);
        services.AddScoped<TContext, TContextSelector>();

        return services;
    }
}
