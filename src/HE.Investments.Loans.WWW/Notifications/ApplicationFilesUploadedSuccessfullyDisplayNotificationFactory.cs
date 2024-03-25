using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class ApplicationFilesUploadedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ApplicationFilesUploadedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        var notificationMessage = "File successfully uploaded";
        if (int.TryParse(notification.Parameters[ApplicationFilesUploadedSuccessfullyNotification.FilesCountParameterName], out var filesCount))
        {
            notificationMessage = filesCount > 1 ? "Files successfully uploaded" : notificationMessage;
        }

        return DisplayNotification.Success(notification.TemplateText(notificationMessage));
    }
}
