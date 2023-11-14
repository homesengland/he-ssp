using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class LoanApplicationHasBeenResubmittedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(LoanApplicationHasBeenResubmittedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("Your application has now been submitted", body: "Your Transaction Manager will review your updated application.");
    }
}
