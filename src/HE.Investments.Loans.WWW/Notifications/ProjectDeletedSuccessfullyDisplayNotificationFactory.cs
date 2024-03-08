using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.Projects.Notifications;

namespace HE.Investments.Loans.WWW.Notifications;

public class ProjectDeletedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ProjectDeletedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{ProjectDeletedSuccessfullyNotification.ProjectNameParameterName}> removed"));
    }
}
