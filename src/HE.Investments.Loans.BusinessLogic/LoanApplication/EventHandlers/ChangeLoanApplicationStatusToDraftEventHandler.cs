using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class ChangeLoanApplicationStatusToDraftEventHandler : IEventHandler<LoanApplicationHasBeenChangedEvent>
{
    private readonly ILoanApplicationRepository _repository;

    public ChangeLoanApplicationStatusToDraftEventHandler(ILoanApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(LoanApplicationHasBeenChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent.CurrentStatus == ApplicationStatus.New)
        {
            await _repository.MoveToDraft(domainEvent.LoanApplicationId, cancellationToken);
        }
    }
}
