using HE.InvestmentLoans.BusinessLogic.Projects.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ProjectDeletedSuccessfullyDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(ProjectDeletedSuccessfullyNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{ProjectDeletedSuccessfullyNotification.ProjectNameParameterName}> removed"));
    }
}
