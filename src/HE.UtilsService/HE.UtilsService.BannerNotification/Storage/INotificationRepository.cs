namespace HE.UtilsService.BannerNotification.Storage;

public interface INotificationRepository
{
    Task<ApplicationAreaNotifications> GetAreaNotifications(string key);

    Task Save(ApplicationAreaNotifications areaNotifications);
}
