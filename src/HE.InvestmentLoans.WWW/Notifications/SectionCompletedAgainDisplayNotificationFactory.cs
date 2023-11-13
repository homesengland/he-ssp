using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class SectionCompletedAgainDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(SectionCompletedAgainNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("The section is now complete", body: "You can update other sections or submit your application.");
    }
}
