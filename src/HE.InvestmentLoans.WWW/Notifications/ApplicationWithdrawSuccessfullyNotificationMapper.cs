using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ApplicationWithdrawSuccessfullyNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(ApplicationWithdrawSuccessfullyNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success(
            notification.TemplateText($"<{ApplicationWithdrawSuccessfullyNotification.ApplicationNameParameterName}> project has been withdrawn."),
            notification.TemplateText(
                $"Contact <{ApplicationWithdrawSuccessfullyNotification.FundingSupportEmailParameterName}> if you think there's a problem."));
    }
}
