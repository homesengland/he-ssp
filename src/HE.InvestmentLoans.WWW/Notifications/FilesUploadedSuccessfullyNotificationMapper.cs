using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class FilesUploadedSuccessfullyNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(FilesUploadedSuccessfullyNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success(notification.TemplateText($"<{FilesUploadedSuccessfullyNotification.FilesParameterName}> successfully uploaded"));
    }
}
