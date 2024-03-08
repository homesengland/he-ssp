using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class ChangeOrganisationDetailsRequestedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ChangeOrganisationDetailsRequestedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("Your request to change your organisation details has been sent for review.");
    }
}
