using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;

namespace HE.Investments.Common.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IDictionary<string, INotificationDisplayMapper> _notificationDisplayMappers;

    private readonly ICacheService _cacheService;

    private readonly string _userNotificationKey;

    public NotificationService(
        ICacheService cacheService,
        IUserContext userContext,
        IEnumerable<INotificationDisplayMapper> notificationDisplayMappers)
    {
        _cacheService = cacheService;
        _userNotificationKey = $"notification-{userContext.UserGlobalId}";
        _notificationDisplayMappers = notificationDisplayMappers.ToDictionary(x => x.HandledNotificationType.Name, x => x);
    }

    public NotificationToDisplay? Pop()
    {
        var notification = _cacheService.GetValue<Notification>(_userNotificationKey);
        if (notification != null)
        {
            _cacheService.SetValue<Notification?>(_userNotificationKey, null);
            return Map(notification);
        }

        return null;
    }

    public async Task Publish(Notification notification)
    {
        await _cacheService.SetValueAsync(_userNotificationKey, notification);
    }

    private NotificationToDisplay Map(Notification notification)
    {
        if (_notificationDisplayMappers.TryGetValue(notification.NotificationType, out var notificationMapper))
        {
            return notificationMapper.Map(notification);
        }

        throw new ArgumentOutOfRangeException(nameof(notification.NotificationType), "Unsupported notification type");
    }
}
