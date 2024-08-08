using System.Text.Json;

namespace HE.UtilsService.BannerNotification.Storage.InMemory;

public class NotificationInMemoryRepository : INotificationRepository
{
    private static readonly Dictionary<string, string> Notifications = [];

    public Task<ApplicationAreaNotifications> GetAreaNotifications(string key)
    {
        if (Notifications.TryGetValue(key, out var areaNotifications))
        {
            return Task.FromResult(new ApplicationAreaNotifications(key, JsonSerializer.Deserialize<NotificationRequest[]>(areaNotifications)));
        }

        return Task.FromResult(new ApplicationAreaNotifications(key, []));
    }

    public Task Save(ApplicationAreaNotifications areaNotifications)
    {
        Notifications[areaNotifications.Key] = JsonSerializer.Serialize(areaNotifications.NotificationRequests);
        return Task.CompletedTask;
    }
}
