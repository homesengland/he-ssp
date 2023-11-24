using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenResubmittedEventHandler : IEventHandler<LoanApplicationHasBeenResubmittedEvent>
{
    private readonly INotificationService _notificationService;

    public LoanApplicationHasBeenResubmittedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(LoanApplicationHasBeenResubmittedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new LoanApplicationHasBeenResubmittedNotification());
    }
}
