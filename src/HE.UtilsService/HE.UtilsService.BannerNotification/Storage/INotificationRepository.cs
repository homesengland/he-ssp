namespace HE.UtilsService.BannerNotification.Storage;

public interface INotificationRepository
{
    Task<ApplicationAreaNotifications> GetAreaNotifications(string key, CancellationToken cancellationToken);

    Task Save(ApplicationAreaNotifications areaNotifications, CancellationToken cancellationToken);
}
