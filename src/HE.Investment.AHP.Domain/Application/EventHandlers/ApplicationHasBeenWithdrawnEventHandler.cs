using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class ApplicationHasBeenWithdrawnEventHandler : IEventHandler<ApplicationHasBeenWithdrawnEvent>
{
    private readonly INotificationService _notificationService;

    public ApplicationHasBeenWithdrawnEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ApplicationHasBeenWithdrawnEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new ApplicationHasBeenWithdrawnNotification());
    }
}
