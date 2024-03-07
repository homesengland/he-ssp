using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class FilesUploadedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(FilesUploadedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{FilesUploadedSuccessfullyNotification.FilesParameterName}> successfully uploaded"));
    }
}
