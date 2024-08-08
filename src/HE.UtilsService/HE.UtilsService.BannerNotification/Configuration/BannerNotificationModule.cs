using HE.UtilsService.BannerNotification.Shared;
using HE.UtilsService.BannerNotification.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace HE.UtilsService.BannerNotification.Configuration;

public static class BannerNotificationModule
{
    public static void AddBannerNotificationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationKeyFactory, NotificationKeyFactory>();
    }
}
