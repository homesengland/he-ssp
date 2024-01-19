using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public class LoanApplicationChangeToDraftStatusEvent : DomainEvent
{
    public LoanApplicationChangeToDraftStatusEvent(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
    }

    public LoanApplicationId LoanApplicationId { get; private set; }
}
