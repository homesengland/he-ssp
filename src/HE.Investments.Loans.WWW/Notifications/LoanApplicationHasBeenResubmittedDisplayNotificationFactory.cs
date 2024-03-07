using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class LoanApplicationHasBeenResubmittedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(LoanApplicationHasBeenResubmittedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success("Your application has now been submitted", body: "Your Transaction Manager will review your updated application.");
    }
}
