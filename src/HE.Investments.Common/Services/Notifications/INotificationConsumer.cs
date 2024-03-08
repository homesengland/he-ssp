namespace HE.Investments.Common.Services.Notifications;

public interface INotificationConsumer
{
    DisplayNotification? Pop();
}
