using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;
using HE.UtilsService.BannerNotification.Shared;

namespace HE.Investments.Common.Services.Notifications;

internal sealed class NotificationPublisher : INotificationPublisher
{
    private readonly ICacheService _cacheService;

    private readonly INotificationKeyFactory _notificationKeyFactory;

    private readonly ApplicationType _application;

    private readonly IUserContext _userContext;

    public NotificationPublisher(ICacheService cacheService, INotificationKeyFactory notificationKeyFactory, ApplicationType application, IUserContext userContext)
    {
        _cacheService = cacheService;
        _notificationKeyFactory = notificationKeyFactory;
        _application = application;
        _userContext = userContext;
    }

    public async Task Publish<TNotification>(TNotification notification)
        where TNotification : Notification
    {
        await Publish(_application, notification);
    }

    public async Task Publish<TNotification>(ApplicationType application, TNotification notification)
        where TNotification : Notification
    {
        var userNotificationKey = _notificationKeyFactory.KeyForUser(_userContext.UserGlobalId, application);
        await _cacheService.SetValueAsync(userNotificationKey, notification);
    }
}
