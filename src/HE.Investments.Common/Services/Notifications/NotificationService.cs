using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;

namespace HE.Investments.Common.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IDictionary<string, IDisplayNotificationFactory> _displayNotificationFactories;

    private readonly ICacheService _cacheService;

    private readonly string _userNotificationKey;

    public NotificationService(
        ICacheService cacheService,
        IUserContext userContext,
        IEnumerable<IDisplayNotificationFactory> displayNotificationFactories,
        string applicationName)
    {
        _cacheService = cacheService;
        _userNotificationKey = $"notification-{applicationName.ToLowerInvariant()}-{userContext.UserGlobalId}";
        _displayNotificationFactories = displayNotificationFactories.ToDictionary(x => x.HandledNotificationType.Name, x => x);
    }

    public DisplayNotification? Pop()
    {
        var notification = _cacheService.GetValue<Notification>(_userNotificationKey);
        if (notification != null)
        {
            _cacheService.Delete(_userNotificationKey);
            return Map(notification);
        }

        return null;
    }

    public async Task Publish(Notification notification)
    {
        await _cacheService.SetValueAsync(_userNotificationKey, notification);
    }

    private DisplayNotification Map(Notification notification)
    {
        if (_displayNotificationFactories.TryGetValue(notification.NotificationType, out var displayNotificationFactory))
        {
            return displayNotificationFactory.Create(notification);
        }

        throw new ArgumentOutOfRangeException(nameof(notification), $"Unsupported notification type: {notification.NotificationType}");
    }
}
