using HE.UtilsService.BannerNotification.Shared;
using HE.UtilsService.BannerNotification.Storage;

namespace HE.UtilsService.BannerNotification;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    private readonly INotificationKeyFactory _notificationKeyFactory;

    public NotificationService(INotificationRepository notificationRepository, INotificationKeyFactory notificationKeyFactory)
    {
        _notificationRepository = notificationRepository;
        _notificationKeyFactory = notificationKeyFactory;
    }

    public async Task PublishNotification(Contract.BannerNotification notification, CancellationToken cancellationToken)
    {
        var areaNotifications = await _notificationRepository.GetAreaNotifications(
                                        _notificationKeyFactory.KeyForOrganisation(
                                                notification.OrganisationId,
                                                notification.ApplicationType,
                                                notification.ApplicationArea));

        areaNotifications.AddNotification(notification.NotificationType, notification.NotificationParameters ?? []);

        await _notificationRepository.Save(areaNotifications);
    }
}
