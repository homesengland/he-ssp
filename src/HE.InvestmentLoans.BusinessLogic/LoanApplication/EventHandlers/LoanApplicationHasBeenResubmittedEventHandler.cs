using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;

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
