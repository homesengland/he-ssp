using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.EventHandlers;

public class ApplicationStatusHasBeenChangedEventHandler : IEventHandler<ApplicationStatusHasBeenChangedEvent>
{
    private readonly INotificationService _notificationService;

    public ApplicationStatusHasBeenChangedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(ApplicationStatusHasBeenChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new ApplicationStatusHasBeenChangedNotification(domainEvent.ApplicationStatus));
    }
}
