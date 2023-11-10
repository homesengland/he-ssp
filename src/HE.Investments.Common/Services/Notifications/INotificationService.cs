namespace HE.Investments.Common.Services.Notifications;

public interface INotificationService
{
    public NotificationToDisplay? Pop();

    public Task Publish(Notification notification);
}
