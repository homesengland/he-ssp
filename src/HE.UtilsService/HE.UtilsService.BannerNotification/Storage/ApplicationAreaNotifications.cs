using HE.UtilsService.BannerNotification.Contract;

namespace HE.UtilsService.BannerNotification.Storage;

public class ApplicationAreaNotifications
{
    public ApplicationAreaNotifications(string key, IList<NotificationRequest>? notificationRequests)
    {
        Key = key;
        NotificationRequests.AddRange(notificationRequests ?? []);
    }

    public string Key { get; }

    public List<NotificationRequest> NotificationRequests { get; } = [];

    public void AddNotification(string notificationType, List<BannerNotificationParameter> parameters)
    {
        NotificationRequests.Add(new NotificationRequest
        {
            NotificationType = notificationType,
            Parameters = parameters.ToDictionary(p => p.Name, p => p.Value),
        });
    }
}
