using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class TestNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => OrganisationNotificationType.TestNotification;

    public DisplayNotification Create(Notification notification)
    {
        var nickname = notification.Parameters["Nickname"];
        return DisplayNotification.Success("Test notification", body: $"And btw, {nickname} is awesome");
    }
}
