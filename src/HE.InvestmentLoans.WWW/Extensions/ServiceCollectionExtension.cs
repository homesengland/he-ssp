using HE.InvestmentLoans.Common.Configuration;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services;
using HE.InvestmentLoans.Common.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
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

    public static void AddCache(this IServiceCollection services, IAppConfig config, IAppConfig appConfig)
    {
        if (string.IsNullOrEmpty(config.Cache.RedisConnectionString) || config.Cache.RedisConnectionString == "off")
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
        else
        {
            var redis = new RedisConfigurationOptions(config.Cache);

            services.AddSingleton<ICacheService>(x => new RedisService(config, redis.ConfigurationOptions));
            services.AddDataProtection()
                .SetApplicationName(appConfig.AppName!)
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redis.ConfigurationOptions), "DataProtection-Keys");

            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "redis";
                action.ConfigurationOptions = redis.ConfigurationOptions;
            });
        }
    }
}
