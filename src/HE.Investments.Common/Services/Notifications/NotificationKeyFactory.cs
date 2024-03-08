using HE.Investments.Common.User;

namespace HE.Investments.Common.Services.Notifications;

internal class NotificationKeyFactory : INotificationKeyFactory
{
    private readonly IUserContext _userContext;

    public NotificationKeyFactory(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public string CreateKey(ApplicationType application)
    {
        return $"notification-{application.ToString().ToLowerInvariant()}-{_userContext.UserGlobalId}";
    }
}
