using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.EventHandlers;

public class SendUserInvitedNotificationEventHandler : IEventHandler<UserInvitedEvent>
{
    private readonly INotificationService _notificationService;

    public SendUserInvitedNotificationEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(UserInvitedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new UserInvitedNotification($"{domainEvent.FirstName} {domainEvent.LastName}"));
    }
}
