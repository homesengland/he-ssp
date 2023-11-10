using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class FileRemovedSuccessfullyNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(FileRemovedSuccessfullyNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success(notification.TemplateText($"<{FileRemovedSuccessfullyNotification.FileParameterName}> successfully removed"));
    }
}
