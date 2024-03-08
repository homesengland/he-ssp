using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class ApplicationHasBeenPutOnHoldEventHandler : IEventHandler<ApplicationHasBeenPutOnHoldEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public ApplicationHasBeenPutOnHoldEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(ApplicationHasBeenPutOnHoldEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new ApplicationHasBeenPutOnHoldNotification());
    }
}
