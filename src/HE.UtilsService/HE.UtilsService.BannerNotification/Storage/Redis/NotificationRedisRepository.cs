using System.Text.Json;
using HE.UtilsService.BannerNotification.Configuration;
using HE.UtilsService.BannerNotification.Shared;
using StackExchange.Redis;

namespace HE.UtilsService.BannerNotification.Storage.Redis;

public class NotificationRedisRepository : INotificationRepository
{
    private const int DefaultExpirationTime = 48;

    private readonly ConnectionMultiplexer _connection;

    private readonly INotificationConfig _notificationConfig;

    public NotificationRedisRepository(INotificationConfig notificationConfig)
    {
        _notificationConfig = notificationConfig;
        var redisConfigurationOptions = new RedisConfigurationOptions(_notificationConfig);
        _connection = ConnectionMultiplexer.Connect(redisConfigurationOptions.ConfigurationOptions);
    }

    private IDatabase StorageContext => _connection.GetDatabase();

    public async Task<ApplicationAreaNotifications> GetAreaNotifications(string key)
    {
        var storage = await StorageContext.StringGetAsync(key);
        if (storage.IsNullOrEmpty)
        {
            return new ApplicationAreaNotifications(key, []);
        }

        return new ApplicationAreaNotifications(key, JsonSerializer.Deserialize<NotificationRequest[]>(storage.ToString()));
    }

    public async Task Save(ApplicationAreaNotifications areaNotifications)
    {
        await StorageContext.StringSetAsync(
            areaNotifications.Key,
            JsonSerializer.Serialize(areaNotifications.NotificationRequests),
            TimeSpan.FromHours(_notificationConfig.NotificationExpirationTimeInHours ?? DefaultExpirationTime));
    }
}
