using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Config;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HE.Investments.Common.WWW.Infrastructure.Cache;

public static class CacheModule
{
    public static void AddCache(this IServiceCollection services, ICacheConfig config, string appName)
    {
        services.AddSingleton<ICacheConfig>(config);
        if (string.IsNullOrEmpty(config.RedisConnectionString) || config.RedisConnectionString == "off")
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
        else
        {
            var redis = new RedisConfigurationOptions(config, appName);

            services.AddSingleton<ICacheService>(x => new RedisService(config, redis.ConfigurationOptions));
            services.AddDataProtection()
                .SetApplicationName(appName)
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redis.ConfigurationOptions), "DataProtection-Keys");

            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "redis";
                action.ConfigurationOptions = redis.ConfigurationOptions;
            });
        }
    }
}
