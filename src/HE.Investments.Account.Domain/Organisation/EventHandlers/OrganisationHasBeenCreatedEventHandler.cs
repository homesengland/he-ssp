using HE.Investments.Account.Contract.Organisation.Events;
using HE.Investments.Account.Domain.Organisation.Notifications;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.Organisation.EventHandlers;

public class OrganisationHasBeenCreatedEventHandler : IEventHandler<OrganisationHasBeenCreatedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public OrganisationHasBeenCreatedEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(OrganisationHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(ApplicationType.Account, ToNotification(domainEvent));
    }

    private static Notification ToNotification(OrganisationHasBeenCreatedEvent domainEvent)
    {
        return new Notification(
            nameof(OrganisationAddedNotification),
            new Dictionary<string, string> { { OrganisationAddedNotification.OrganisationNameParameterName, domainEvent.OrganisationName } });
    }
}
