using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.Notifications;

public class UserUnlinkedNotification : Notification
{
    public const string UserFullNameParameterName = "UserFullName";

    public UserUnlinkedNotification(string userFullName)
        : base(new Dictionary<string, string> { { UserFullNameParameterName, userFullName } })
    {
    }
}
