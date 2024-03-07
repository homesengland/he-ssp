using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class FrontDoorProjectHasBeenCreatedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => "FrontDoorProjectHasBeenCreatedNotification";

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText("<ProjectName> has been saved as a draft."));
    }
}
