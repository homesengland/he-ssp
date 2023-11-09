namespace HE.Investments.Common.Services.Notifications;

public interface INotificationService
{
    public Task NotifySuccess(NotificationBodyType notificationBodyType, IDictionary<NotificationServiceKeys, string>? valuesToDisplay);

    public Tuple<bool, NotificationModel?> Pop();
}
