using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
public interface ICanSubmitLoanApplication
{
    Task Submit(LoanApplicationId loanApplicationId, CancellationToken cancellationToken);
}
