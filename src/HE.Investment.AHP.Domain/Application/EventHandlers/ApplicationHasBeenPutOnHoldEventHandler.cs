using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class ApplicationHasBeenPutOnHoldEventHandler : IEventHandler<ApplicationHasBeenPutOnHoldEvent>
{
    private readonly INotificationService _notificationService;

    public ApplicationHasBeenPutOnHoldEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ApplicationHasBeenPutOnHoldEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new ApplicationHasBeenPutOnHoldNotification());
    }
}
