using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class FilesUploadedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(FilesUploadedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{FilesUploadedSuccessfullyNotification.FilesParameterName}> successfully uploaded"));
    }
}
