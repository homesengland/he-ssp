using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class UserUnlinkedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(UserUnlinkedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{UserInvitedNotification.UserFullNameParameterName}> removed"));
    }
}
