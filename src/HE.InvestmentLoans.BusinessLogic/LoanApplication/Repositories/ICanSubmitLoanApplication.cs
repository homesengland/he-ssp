using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
public interface ICanSubmitLoanApplication
{
    void Submit(LoanApplicationId loanApplicationId, CancellationToken cancellationToken);
}
