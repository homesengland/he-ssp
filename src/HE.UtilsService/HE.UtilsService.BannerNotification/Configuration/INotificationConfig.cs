using HE.UtilsService.BannerNotification.Storage.Redis;

namespace HE.UtilsService.BannerNotification.Configuration;

public interface INotificationConfig : IRedisConfig
{
    int? NotificationExpirationTimeInHours { get; }
}
