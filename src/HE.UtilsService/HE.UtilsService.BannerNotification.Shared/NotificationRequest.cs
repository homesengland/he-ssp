namespace HE.UtilsService.BannerNotification.Shared;

public class NotificationRequest
{
    public string NotificationType { get; set; }

    public Dictionary<string, string> Parameters { get; set; }
}
