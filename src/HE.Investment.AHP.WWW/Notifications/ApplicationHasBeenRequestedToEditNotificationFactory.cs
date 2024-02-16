using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class ApplicationHasBeenRequestedToEditNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(ApplicationHasBeenRequestedToEditNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            "Your request to edit has been sent.",
            body: "You’ll be notified once your Growth Manager has referred your application back to you.");
    }
}
