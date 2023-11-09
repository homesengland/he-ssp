using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenResubmittedEventHandler : IEventHandler<LoanApplicationHasBeenResubmittedEvent>
{
    private readonly INotificationService _notificationService;

    public LoanApplicationHasBeenResubmittedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(LoanApplicationHasBeenResubmittedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.NotifySuccess(NotificationBodyType.ApplicationResubmitted);
    }
}
