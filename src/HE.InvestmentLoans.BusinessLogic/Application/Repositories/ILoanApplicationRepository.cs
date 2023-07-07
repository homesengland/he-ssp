using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Application.Repositories;

public interface ILoanApplicationRepository
{
    void Save(LoanApplicationViewModel loanApplication, UserAccount userAccount);
}
