namespace HE.UtilsService.BannerNotification.Shared;

public class NotificationRequest
{
    public string NotificationType { get; set; }

    public IDictionary<string, string> Parameters { get; set; }
}
