using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.Investments.Organisation.CompaniesHouse;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HE.InvestmentLoans.WWW.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAppConfig>(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
        services.AddSingleton<IDataverseConfig>(x => x.GetRequiredService<IAppConfig>().Dataverse);
        services.AddSingleton<IUrlConfig>(x => x.GetRequiredService<IAppConfig>().Url);
        services.AddSingleton<ICacheConfig>(x => x.GetRequiredService<IAppConfig>().Cache);

        services.AddSingleton<ICompaniesHouseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetRequiredSection("AppConfiguration:CompaniesHouse").Get<CompaniesHouseConfig>());
    }

    public static void AddCache(this IServiceCollection services, ICacheConfig config, string sessionCookieName)
    {
        if (!string.IsNullOrEmpty(config.RedisConnectionString))
        {
            services.AddSingleton<ICacheService, RedisService>();
            services.AddDataProtection()
                    .SetApplicationName(sessionCookieName)
                    .PersistKeysToStackExchangeRedis(
                        ConnectionMultiplexer.Connect(config.RedisConnectionString),
                        "DataProtection-Keys");

            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "redis";
                action.Configuration = config.RedisConnectionString;
            });
        }
        else
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
    }
}
