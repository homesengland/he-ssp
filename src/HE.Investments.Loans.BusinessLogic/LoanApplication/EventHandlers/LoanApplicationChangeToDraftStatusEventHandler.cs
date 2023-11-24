using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationChangeToDraftStatusEventHandler : IEventHandler<LoanApplicationChangeToDraftStatusEvent>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public LoanApplicationChangeToDraftStatusEventHandler(ILoanApplicationRepository loanApplicationRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task Handle(LoanApplicationChangeToDraftStatusEvent domainEvent, CancellationToken cancellationToken)
    {
        await _loanApplicationRepository.MoveToDraft(domainEvent.LoanApplicationId, cancellationToken);
    }
}
