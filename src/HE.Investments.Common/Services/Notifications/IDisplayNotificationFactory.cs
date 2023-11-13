namespace HE.Investments.Common.Services.Notifications;

public interface IDisplayNotificationFactory
{
    Type HandledNotificationType { get; }

    DisplayNotification Create(Notification notification);
}
