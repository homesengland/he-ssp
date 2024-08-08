using HE.UtilsService.BannerNotification.Shared;
using HE.UtilsService.BannerNotification.Storage;
using HE.UtilsService.BannerNotification.Storage.InMemory;
using HE.UtilsService.BannerNotification.Storage.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace HE.UtilsService.BannerNotification.Configuration;

public static class BannerNotificationModule
{
    public static void AddBannerNotificationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationRepository>(x =>
        {
            var config = x.GetRequiredService<INotificationConfig>();
            if (string.IsNullOrWhiteSpace(config.RedisConnectionString) || config.RedisConnectionString == "off")
            {
                return new NotificationInMemoryRepository();
            }

            return new NotificationRedisRepository(config);
        });
        services.AddScoped<INotificationKeyFactory, NotificationKeyFactory>();
    }
}
