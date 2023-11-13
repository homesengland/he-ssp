using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ApplicationWithdrawSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(ApplicationWithdrawSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            notification.TemplateText($"<{ApplicationWithdrawSuccessfullyNotification.ApplicationNameParameterName}> project has been withdrawn."),
            notification.TemplateText(
                $"Contact <{ApplicationWithdrawSuccessfullyNotification.FundingSupportEmailParameterName}> if you think there's a problem."));
    }
}
