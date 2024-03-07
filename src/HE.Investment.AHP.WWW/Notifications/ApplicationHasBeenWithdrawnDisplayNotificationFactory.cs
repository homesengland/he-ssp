using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class ApplicationHasBeenWithdrawnDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ApplicationHasBeenWithdrawnNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            "Your application has been withdrawn.",
            body: "You will no longer be able proceed with this application.");
    }
}
