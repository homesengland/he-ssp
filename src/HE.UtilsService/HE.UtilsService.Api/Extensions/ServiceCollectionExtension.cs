using HE.UtilsService.Api.Configuration;
using HE.UtilsService.BannerNotification.Configuration;
using HE.UtilsService.BannerNotification.Storage.Redis;
using HE.UtilsService.SharePoint.Configuration;
using Microsoft.Extensions.Options;

namespace HE.UtilsService.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddConfigs(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfig>(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
        services.AddSingleton<ISharePointConfiguration>(x => x.GetRequiredService<IAppConfig>().SharePoint);
        services.AddSingleton<INotificationConfig>(x => x.GetRequiredService<IAppConfig>().Notification);
        services.AddSingleton<IRedisConfig>(x => x.GetRequiredService<IAppConfig>().Notification);
    }
}
