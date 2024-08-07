namespace HE.UtilsService.BannerNotification;

public interface INotificationService
{
    Task PublishNotification(Contract.BannerNotification notification, CancellationToken cancellationToken);
}
