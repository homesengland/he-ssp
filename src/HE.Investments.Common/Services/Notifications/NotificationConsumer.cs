using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;

namespace HE.Investments.Common.Services.Notifications;

internal sealed class NotificationConsumer : INotificationConsumer
{
    private readonly ICacheService _cacheService;

    private readonly Dictionary<string, IDisplayNotificationFactory> _displayNotificationFactories;

    private readonly string _userNotificationKey;

    public NotificationConsumer(
        ICacheService cacheService,
        INotificationKeyFactory notificationKeyFactory,
        IEnumerable<IDisplayNotificationFactory> displayNotificationFactories,
        ApplicationType application)
    {
        _cacheService = cacheService;
        _displayNotificationFactories = displayNotificationFactories.ToDictionary(x => x.HandledNotificationType, x => x);
        _userNotificationKey = notificationKeyFactory.CreateKey(application);
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

    private DisplayNotification Map(Notification notification)
    {
        if (_displayNotificationFactories.TryGetValue(notification.NotificationType, out var displayNotificationFactory))
        {
            return displayNotificationFactory.Create(notification);
        }

        throw new ArgumentOutOfRangeException(nameof(notification), $"Unsupported notification type: {notification.NotificationType}");
    }
}
