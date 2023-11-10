using HE.InvestmentLoans.BusinessLogic.LoanApplication.Notifications;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationSectionHasBeenCompletedAgainEventHandler : IEventHandler<LoanApplicationSectionHasBeenCompletedAgainEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ILoanUserContext _loanUserContext;

    public LoanApplicationSectionHasBeenCompletedAgainEventHandler(
        INotificationService notificationService,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext)
    {
        _notificationService = notificationService;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(LoanApplicationSectionHasBeenCompletedAgainEvent domainEvent, CancellationToken cancellationToken)
    {
        var loanApplication =
            await _loanApplicationRepository.GetLoanApplication(domainEvent.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        if (loanApplication.WasSubmitted())
        {
            await _notificationService.Publish(new SectionCompletedAgainNotification());
        }
    }
}
