using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.HomeTypes.Notifications;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class HomeTypeHasBeenCreatedEventHandler : IEventHandler<HomeTypeHasBeenCreatedEvent>
{
    private readonly INotificationService _notificationService;

    public HomeTypeHasBeenCreatedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new HomeTypeHasBeenCreatedNotification(domainEvent.HomeTypeName ?? "Unknown Home Type"));
    }
}
