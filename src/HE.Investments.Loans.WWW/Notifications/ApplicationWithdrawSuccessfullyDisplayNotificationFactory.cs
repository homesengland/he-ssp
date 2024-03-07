using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class ApplicationWithdrawSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ApplicationWithdrawSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            notification.TemplateText($"<{ApplicationWithdrawSuccessfullyNotification.ApplicationNameParameterName}> project has been withdrawn."),
            notification.TemplateText(
                $"Contact <{ApplicationWithdrawSuccessfullyNotification.FundingSupportEmailParameterName}> if you think there's a problem."));
    }
}
