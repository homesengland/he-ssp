using HE.UtilsService.BannerNotification.Shared;

namespace HE.Investments.Common.Services.Notifications;

public interface INotificationPublisher
{
    Task Publish<TNotification>(TNotification notification)
        where TNotification : Notification;

    Task Publish<TNotification>(ApplicationType application, TNotification notification)
        where TNotification : Notification;
}
