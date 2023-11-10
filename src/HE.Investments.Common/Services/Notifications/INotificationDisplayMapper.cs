namespace HE.Investments.Common.Services.Notifications;

public interface INotificationDisplayMapper
{
    Type HandledNotificationType { get; }

    NotificationToDisplay Map(Notification notification);
}
