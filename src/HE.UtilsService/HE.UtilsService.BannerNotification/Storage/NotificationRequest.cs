namespace HE.UtilsService.BannerNotification.Storage;

public class NotificationRequest
{
    public string NotificationType { get; set; }

    public IDictionary<string, string> Parameters { get; set; }
}
