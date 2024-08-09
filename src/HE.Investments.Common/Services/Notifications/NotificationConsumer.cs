using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;
using HE.UtilsService.BannerNotification.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Common.Services.Notifications;

internal sealed class NotificationConsumer : INotificationConsumer
{
    private readonly ICacheService _cacheService;

    private readonly INotificationKeyFactory _notificationKeyFactory;

    private readonly ApplicationType _application;

    private readonly IUserContext _userContext;

    private readonly Dictionary<string, IDisplayNotificationFactory> _displayNotificationFactories;

    private readonly string _userNotificationKey;

    private readonly ILogger<NotificationConsumer> _logger;

    public NotificationConsumer(
        ICacheService cacheService,
        INotificationKeyFactory notificationKeyFactory,
        IEnumerable<IDisplayNotificationFactory> displayNotificationFactories,
        ApplicationType application,
        IUserContext userContext,
        ILogger<NotificationConsumer> logger)
    {
        _cacheService = cacheService;
        _notificationKeyFactory = notificationKeyFactory;
        _application = application;
        _userContext = userContext;
        _logger = logger;
        _displayNotificationFactories = displayNotificationFactories.ToDictionary(x => x.HandledNotificationType, x => x);
        _userNotificationKey = _notificationKeyFactory.KeyForUser(_userContext.UserGlobalId, application);
    }

    public DisplayNotification[] Pop(ApplicationArea? applicationArea)
    {
        var displayNotifications = new List<DisplayNotification>();
        var notification = _cacheService.GetValue<NotificationRequest>(_userNotificationKey);
        if (notification != null)
        {
            _cacheService.Delete(_userNotificationKey);
            var mappedNotification = Map(notification);
            if (mappedNotification != null)
            {
                displayNotifications.Add(mappedNotification);
            }
        }

        var areaNotificationKey = _notificationKeyFactory.KeyForOrganisation(_userContext.OrganisationId!.ToGuidAsString(), _application, applicationArea);
        var organisationNotifications = _cacheService.GetValue<NotificationRequest[]>(areaNotificationKey);
        if (organisationNotifications is { Length: > 0 })
        {
            _cacheService.Delete(areaNotificationKey);
            displayNotifications.AddRange(organisationNotifications.Select(Map).Where(x => x != null).Select(x => x!).ToList());
        }

        return displayNotifications.ToArray();
    }

    private DisplayNotification? Map(NotificationRequest notificationRequest)
    {
        if (_displayNotificationFactories.TryGetValue(notificationRequest.NotificationType, out var displayNotificationFactory))
        {
            return displayNotificationFactory.Create(new Notification(notificationRequest.NotificationType, notificationRequest.Parameters));
        }

        _logger.LogError("Unsupported notification type: {NotificationType}", notificationRequest.NotificationType);
        return null;
    }
}
