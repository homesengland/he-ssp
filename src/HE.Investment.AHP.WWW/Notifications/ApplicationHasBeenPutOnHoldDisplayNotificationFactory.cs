using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class ApplicationHasBeenPutOnHoldDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(ApplicationHasBeenPutOnHoldNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            "Your application has been put on hold.",
            body: "You’ll be able to reactivate and submit this application at any time.");
    }
}
