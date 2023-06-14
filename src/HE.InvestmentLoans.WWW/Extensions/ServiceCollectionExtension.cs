using HE.InvestmentLoans.Common.Models.App;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HE.InvestmentLoans.WWW.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddConfigs(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfig>(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
        services.AddSingleton<IDataverseConfig>(x => x.GetRequiredService<IAppConfig>().Dataverse);
        services.AddSingleton<IUrlConfig>(x => x.GetRequiredService<IAppConfig>().Url);
    }

    public static void AddRedis(this IServiceCollection services, string redisConnectionString, string sessionCookieName)
    {
        services.AddDataProtection()
                .SetApplicationName(sessionCookieName)
                .PersistKeysToStackExchangeRedis(
                    ConnectionMultiplexer.Connect(redisConnectionString),
                    "DataProtection-Keys");

        services.AddStackExchangeRedisCache(action =>
        {
            action.InstanceName = "redis";
            action.Configuration = redisConnectionString;
        });
    }
}
