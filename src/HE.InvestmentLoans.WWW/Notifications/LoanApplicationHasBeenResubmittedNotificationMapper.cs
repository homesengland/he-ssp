using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class LoanApplicationHasBeenResubmittedNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(LoanApplicationHasBeenResubmittedNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success("Your application has now been submitted", body: "Your Transaction Manager will review your updated application.");
    }
}
