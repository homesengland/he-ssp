using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services;
using HE.InvestmentLoans.Common.Services.Interfaces;
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
        services.AddSingleton<IRedisConfig>(x => x.GetRequiredService<IAppConfig>().Redis);
    }

    public static void AddRedis(this IServiceCollection services, IRedisConfig config, string sessionCookieName)
    {
        services.AddSingleton<ICacheService, RedisService>();

        services.AddDataProtection()
                .SetApplicationName(sessionCookieName)
                .PersistKeysToStackExchangeRedis(
                    ConnectionMultiplexer.Connect(config.ConnectionString),
                    "DataProtection-Keys");

        services.AddStackExchangeRedisCache(action =>
        {
            action.InstanceName = "redis";
            action.Configuration = config.ConnectionString;
        });
    }
}
