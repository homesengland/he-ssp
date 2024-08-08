namespace HE.UtilsService.BannerNotification.Storage;

public class NotificationRepository : INotificationRepository
{
    private readonly Dictionary<string, NotificationRequest[]> _inMemoryStorage = [];

    public Task<ApplicationAreaNotifications> GetAreaNotifications(string key, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ApplicationAreaNotifications(key, _inMemoryStorage.TryGetValue(key, out var requests) ? requests : []));
    }

    public Task Save(ApplicationAreaNotifications areaNotifications, CancellationToken cancellationToken)
    {
        _inMemoryStorage[areaNotifications.Key] = [.. areaNotifications.NotificationRequests];
        return Task.CompletedTask;
    }
}
