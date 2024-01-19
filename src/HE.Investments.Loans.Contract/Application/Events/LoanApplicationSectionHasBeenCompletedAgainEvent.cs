using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public class LoanApplicationSectionHasBeenCompletedAgainEvent : DomainEvent
{
    public LoanApplicationSectionHasBeenCompletedAgainEvent(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
    }

    public LoanApplicationId LoanApplicationId { get; private set; }
}
