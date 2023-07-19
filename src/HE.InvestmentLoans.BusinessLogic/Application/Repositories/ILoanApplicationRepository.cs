using HE.InvestmentLoans.BusinessLogic.Application.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Application.Repositories;

public interface ILoanApplicationRepository
{
    LoanApplicationEntity Load(LoanApplicationId id, UserAccount userAccount);

    Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount);

    void Save(LoanApplicationViewModel loanApplication, UserAccount userAccount);
}
