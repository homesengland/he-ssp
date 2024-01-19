using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public class LoanApplicationHasBeenWithdrawnEvent : DomainEvent
{
    public LoanApplicationHasBeenWithdrawnEvent(LoanApplicationId loanApplicationId, LoanApplicationName applicationName)
    {
        LoanApplicationId = loanApplicationId;
        ApplicationName = applicationName;
    }

    public LoanApplicationId LoanApplicationId { get; private set; }

    public LoanApplicationName ApplicationName { get; private set; }
}
