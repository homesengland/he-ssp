using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ChangeOrganisationDetailsRequestedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(ChangeOrganisationDetailsRequestedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("Your request to change your organisation details has been sent for review.");
    }
}
