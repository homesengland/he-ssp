using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Delivery.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Delivery.EventHandlers;

public class SendDeliveryPhaseHasBeenCreatedNotificationEventHandler : IEventHandler<DeliveryPhaseHasBeenCreatedEvent>
{
    private readonly INotificationService _notificationService;

    public SendDeliveryPhaseHasBeenCreatedNotificationEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(DeliveryPhaseHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new DeliveryPhaseHasBeenCreatedNotification(domainEvent.DeliveryPhaseName));
    }
}
