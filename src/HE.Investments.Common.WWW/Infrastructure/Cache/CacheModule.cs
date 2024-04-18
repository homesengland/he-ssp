using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Config;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HE.Investments.Common.WWW.Infrastructure.Cache;

public static class CacheModule
{
    private const string AppName = "InvestmentsProgramme";

    public static void AddCache(this IServiceCollection services, ICacheConfig config)
    {
        services.AddSingleton(config);
        if (string.IsNullOrEmpty(config.RedisConnectionString) || config.RedisConnectionString == "off")
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
        else
        {
            var redis = new RedisConfigurationOptions(config, AppName);

            services.AddSingleton<ICacheService>(x => new RedisService(config, redis.ConfigurationOptions, x.GetRequiredService<ILogger<RedisService>>()));
            services.AddDataProtection()
                .SetApplicationName(AppName)
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redis.ConfigurationOptions), "DataProtection-Keys");

            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "redis";
                action.ConfigurationOptions = redis.ConfigurationOptions;
            });
        }
    }
}
