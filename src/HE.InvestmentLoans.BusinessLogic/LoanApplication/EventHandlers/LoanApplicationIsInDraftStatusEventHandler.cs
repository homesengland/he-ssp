using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationIsInDraftStatusEventHandler : IEventHandler<LoanApplicationIsInDraftStatusEvent>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public LoanApplicationIsInDraftStatusEventHandler(ILoanApplicationRepository loanApplicationRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task Handle(LoanApplicationIsInDraftStatusEvent domainEvent, CancellationToken cancellationToken)
    {
        await _loanApplicationRepository.MoveToDraft(domainEvent.LoanApplicationId, cancellationToken);
    }
}
