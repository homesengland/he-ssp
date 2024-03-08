using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.HomeTypes.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class SendHomeTypeHasBeenCreatedNotificationEventHandler : IEventHandler<HomeTypeHasBeenCreatedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendHomeTypeHasBeenCreatedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new HomeTypeHasBeenCreatedNotification(domainEvent.HomeTypeName));
    }
}
