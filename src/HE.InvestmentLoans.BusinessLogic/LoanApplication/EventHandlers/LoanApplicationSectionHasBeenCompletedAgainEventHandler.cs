using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationSectionHasBeenCompletedAgainEventHandler : IEventHandler<LoanApplicationSectionHasBeenCompletedAgainEvent>
{
    private readonly INotificationService _notificationService;

    public LoanApplicationSectionHasBeenCompletedAgainEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(LoanApplicationSectionHasBeenCompletedAgainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.NotifySuccess(NotificationBodyType.SectionCompletedAgain);
    }
}
