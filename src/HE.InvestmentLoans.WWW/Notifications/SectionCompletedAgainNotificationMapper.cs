using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class SectionCompletedAgainNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(SectionCompletedAgainNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success("The section is now complete", body: "You can update other sections or submit your application.");
    }
}
