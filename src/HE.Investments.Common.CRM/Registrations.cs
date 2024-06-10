using System.IdentityModel.Tokens.Jwt;
using HE.Investments.Common.CRM.Infrastructure;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Infrastructure.HealthChecks;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Common.CRM;

public static class Registrations
{
    public static void AddCrmConnection(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationServiceAsync2>(serviceProvider =>
        {
            const string cacheKey = "OrganizationServiceAuthToken";
            var cacheService = serviceProvider.GetRequiredService<ICacheService>();
            var cachedAuthToken = cacheService.GetValue<string?>(cacheKey);
            var config = serviceProvider.GetRequiredService<IDataverseConfig>();

            if (string.IsNullOrEmpty(cachedAuthToken))
            {
                return ServiceClientFromConnectionString(serviceProvider, config, cacheService, cacheKey);
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(cachedAuthToken);
            var dateTimeProvider = serviceProvider.GetRequiredService<IDateTimeProvider>();
            if (IsTokenExpired(token, dateTimeProvider))
            {
                return ServiceClientFromConnectionString(serviceProvider, config, cacheService, cacheKey);
            }

            return new ServiceClient(
                new Uri(config.BaseUri!),
                _ => Task.FromResult(cachedAuthToken));
        });
        services.AddScoped<ICrmService, CrmService>();
        services.Decorate<ICrmService, CrmServiceExceptionDecorator>();
        services.AddScoped<ICrmConnectionTester, CrmConnectionTester>();
    }

    public static IServiceCollection AddApiCrmContext<TCrmContext, TCrmContextImpl, THttpClient, TSelector>(this IServiceCollection services)
        where TCrmContext : class
        where TCrmContextImpl : class, TCrmContext
        where THttpClient : CrmApiHttpClient
        where TSelector : class, TCrmContext
    {
        services.AddScoped<TCrmContextImpl>();
        services.AddHttpClient<THttpClient>().ConfigureCrmApiHttpClient();
        services.AddScoped<Func<TCrmContextImpl>>(x => x.GetRequiredService<TCrmContextImpl>);
        services.AddScoped<Func<THttpClient>>(x => x.GetRequiredService<THttpClient>);
        services.AddScoped<TCrmContext, TSelector>();

        services.TryAddSingleton<CrmApiTokenProvider>();
        services.TryAddScoped<ICrmApiTokenProvider, CachedCrmApiTokenProvider>();

        return services;
    }

    private static bool IsTokenExpired(JwtSecurityToken token, IDateTimeProvider dateTimeProvider)
    {
        return dateTimeProvider.Now.ToUniversalTime().IsAfter(token.ValidTo.AddMinutes(-10).ToUniversalTime());
    }

    private static ServiceClient ServiceClientFromConnectionString(
        IServiceProvider serviceProvider,
        IDataverseConfig config,
        ICacheService cacheService,
        string cacheKey)
    {
        var connectionString = $@"
                    AuthType=ClientSecret;
                    Url={config.BaseUri};
                    ClientId={config.ClientId};
                    ClientSecret={config.ClientSecret};";

        var serviceClient = new ServiceClient(connectionString, serviceProvider.GetRequiredService<ILogger<ServiceClient>>());
        cacheService.SetValue(cacheKey, serviceClient.CurrentAccessToken);
        return serviceClient;
    }

    private static void ConfigureCrmApiHttpClient(this IHttpClientBuilder builder)
    {
        builder.ConfigureHttpClient((services, client) =>
        {
            var config = services.GetRequiredService<ICrmApiConfig>();
            if (!string.IsNullOrEmpty(config.BaseUri))
            {
                client.BaseAddress = new Uri(config.BaseUri);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config.SubscriptionKey);
            }
        });
    }
}
