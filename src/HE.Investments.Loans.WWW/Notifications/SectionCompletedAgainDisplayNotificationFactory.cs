using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class SectionCompletedAgainDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(SectionCompletedAgainNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("The section is now complete", body: "You can update other sections or submit your application.");
    }
}
