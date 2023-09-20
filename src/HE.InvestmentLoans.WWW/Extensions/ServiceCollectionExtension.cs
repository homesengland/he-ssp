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
        services.AddSingleton<ICacheConfig>(x => x.GetRequiredService<IAppConfig>().Cache);
    }

    public static void AddCache(this IServiceCollection services, ICacheConfig config, IAppConfig appConfig)
    {
        if (string.IsNullOrEmpty(config.RedisConnectionString) || config.RedisConnectionString == "off")
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
        else
        {
            services.AddSingleton<ICacheService, RedisService>();
            services.AddDataProtection()
                .SetApplicationName(appConfig.AppName!)
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(config.RedisConnectionString), "DataProtection-Keys");

            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "redis";
                action.Configuration = config.RedisConnectionString;
            });
        }
    }
}
