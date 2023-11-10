using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ChangeOrganisationDetailsRequestedNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(ChangeOrganisationDetailsRequestedNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success("Your request to change your organisation details has been sent for review.");
    }
}
