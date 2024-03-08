using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class ApplicationHasBeenWithdrawnEventHandler : IEventHandler<ApplicationHasBeenWithdrawnEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public ApplicationHasBeenWithdrawnEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(ApplicationHasBeenWithdrawnEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new ApplicationHasBeenWithdrawnNotification());
    }
}
