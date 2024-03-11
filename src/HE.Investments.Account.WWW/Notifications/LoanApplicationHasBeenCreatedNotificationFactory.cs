using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class LoanApplicationHasBeenCreatedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => "LoanApplicationHasBeenCreatedNotification";

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText("<ApplicationName> has been added as a new Levelling Up Home Building Fund application."));
    }
}
