using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.EventHandlers;

public class SendUserUnlinkedNotificationEventHandler : IEventHandler<UserUnlinkedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendUserUnlinkedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(UserUnlinkedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new UserUnlinkedNotification($"{domainEvent.FirstName} {domainEvent.LastName}"));
    }
}
