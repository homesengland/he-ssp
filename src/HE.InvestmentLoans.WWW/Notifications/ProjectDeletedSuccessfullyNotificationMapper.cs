using HE.InvestmentLoans.BusinessLogic.Projects.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.WWW.Notifications;

public class ProjectDeletedSuccessfullyNotificationMapper : INotificationDisplayMapper
{
    public Type HandledNotificationType => typeof(ProjectDeletedSuccessfullyNotification);

    public NotificationToDisplay Map(Notification notification)
    {
        return NotificationToDisplay.Success(notification.TemplateText($"<{ProjectDeletedSuccessfullyNotification.ProjectNameParameterName}> removed"));
    }
}
