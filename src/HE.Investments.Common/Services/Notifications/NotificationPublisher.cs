using HE.Investments.Common.Infrastructure.Cache.Interfaces;

namespace HE.Investments.Common.Services.Notifications;

internal class NotificationPublisher : INotificationPublisher
{
    private readonly ICacheService _cacheService;

    private readonly INotificationKeyFactory _notificationKeyFactory;

    private readonly ApplicationType _application;

    public NotificationPublisher(ICacheService cacheService, INotificationKeyFactory notificationKeyFactory, ApplicationType application)
    {
        _cacheService = cacheService;
        _notificationKeyFactory = notificationKeyFactory;
        _application = application;
    }

    public async Task Publish<TNotification>(TNotification notification)
        where TNotification : Notification
    {
        await Publish(_application, notification);
    }

    public async Task Publish<TNotification>(ApplicationType application, TNotification notification)
        where TNotification : Notification
    {
        var userNotificationKey = _notificationKeyFactory.CreateKey(application);
        await _cacheService.SetValueAsync(userNotificationKey, notification);
    }
}
