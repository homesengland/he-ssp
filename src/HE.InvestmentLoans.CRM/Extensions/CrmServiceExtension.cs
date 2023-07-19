using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using HE.InvestmentLoans.Common.Models.App;
using Microsoft.Xrm.Sdk;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.CRM.Extensions;

public static class CrmServiceExtension
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
                uri => Task.FromResult(cachedAuthToken));
        });
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
}
