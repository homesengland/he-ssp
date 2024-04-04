using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.FrontDoor.Contract.Project.Events;

namespace HE.Investments.FrontDoor.Domain.Project.EventHandlers;

public class PublishProjectHasBeenCreatedNotificationEventHandler : IEventHandler<FrontDoorProjectHasBeenCreatedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public PublishProjectHasBeenCreatedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(FrontDoorProjectHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(ApplicationType.Account, ToNotification(domainEvent));
    }

    private static Notification ToNotification(FrontDoorProjectHasBeenCreatedEvent domainEvent)
    {
        return new Notification(
            "FrontDoorProjectHasBeenCreatedNotification",
            new Dictionary<string, string> { { "ProjectName", domainEvent.ProjectName } });
    }
}
