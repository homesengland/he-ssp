using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public class LoanApplicationHasBeenStartedEvent : DomainEvent
{
    public LoanApplicationHasBeenStartedEvent(Guid loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
    }

    public Guid LoanApplicationId { get; private set; }
}
