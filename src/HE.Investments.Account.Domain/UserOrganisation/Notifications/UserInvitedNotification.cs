using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.Notifications;

public class UserInvitedNotification : Notification
{
    public const string UserFullNameParameterName = "UserFullName";

    public UserInvitedNotification(string userFullName)
        : base(new Dictionary<string, string> { { UserFullNameParameterName, userFullName } })
    {
    }
}
