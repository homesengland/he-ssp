using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class FileRemovedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(FileRemovedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{FileRemovedSuccessfullyNotification.FileParameterName}> successfully removed"));
    }
}
