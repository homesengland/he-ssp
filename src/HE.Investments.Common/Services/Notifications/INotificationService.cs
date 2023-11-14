namespace HE.Investments.Common.Services.Notifications;

public interface INotificationService
{
    public DisplayNotification? Pop();

    public Task Publish(Notification notification);
}
