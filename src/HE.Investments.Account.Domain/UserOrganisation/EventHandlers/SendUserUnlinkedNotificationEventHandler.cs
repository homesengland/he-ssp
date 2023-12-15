using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.UserOrganisation.EventHandlers;

public class SendUserUnlinkedNotificationEventHandler : IEventHandler<UserUnlinkedEvent>
{
    private readonly INotificationService _notificationService;

    public SendUserUnlinkedNotificationEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(UserUnlinkedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new UserUnlinkedNotification($"{domainEvent.FirstName} {domainEvent.LastName}"));
    }
}
