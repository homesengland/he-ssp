using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.EventHandlers;

public class SendUserInvitedNotificationEventHandler : IEventHandler<UserInvitedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendUserInvitedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(UserInvitedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new UserInvitedNotification($"{domainEvent.FirstName} {domainEvent.LastName}"));
    }
}
