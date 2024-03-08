using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class UserInvitedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(UserInvitedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"Invite sent to <{UserInvitedNotification.UserFullNameParameterName}>"));
    }
}
