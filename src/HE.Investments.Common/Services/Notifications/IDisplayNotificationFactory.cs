namespace HE.Investments.Common.Services.Notifications;

public interface IDisplayNotificationFactory
{
    string HandledNotificationType { get; }

    DisplayNotification Create(Notification notification);
}
