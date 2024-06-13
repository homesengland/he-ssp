using HE.Investments.Api.Auth;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HE.Investments.Api.Config;

public static class Module
{
    public static IServiceCollection AddApiModule(this IServiceCollection services)
    {
        services.TryAddSingleton<ApiTokenProvider>();
        services.TryAddScoped<IApiTokenProvider, CachedApiTokenProvider>();

        services.AddAppConfiguration<IApiConfig, ApiConfig>("InvestmentsApi");
        services.AddAppConfiguration<IApiAuthConfig, ApiAuthConfig>("InvestmentsApi:Auth");

        return services;
    }
}
