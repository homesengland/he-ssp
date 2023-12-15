using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class UserUnlinkedNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(UserUnlinkedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{UserInvitedNotification.UserFullNameParameterName}> removed"));
    }
}
