using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class FileRemovedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(FileRemovedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{FileRemovedSuccessfullyNotification.FileParameterName}> successfully removed"));
    }
}
