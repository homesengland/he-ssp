using HE.Investments.AHP.Allocation.Contract.Claims.Events;
using HE.Investments.AHP.Allocation.Contract.Claims.Notifications;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.AHP.Allocation.Domain.Claims.EventHandlers;

public class ClaimHasBeenSubmittedEventHandler : IEventHandler<ClaimHasBeenSubmittedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public ClaimHasBeenSubmittedEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(ClaimHasBeenSubmittedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new ClaimHasBeenSubmittedNotification(domainEvent.MilestoneType.GetDescription()));
    }
}
