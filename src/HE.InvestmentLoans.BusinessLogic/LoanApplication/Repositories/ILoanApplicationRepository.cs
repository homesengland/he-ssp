using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public interface ILoanApplicationRepository
{
    LoanApplicationEntity Load(LoanApplicationId id, UserAccount userAccount);

    IList<UserLoanApplication> LoadAllLoanApplications(UserAccount userAccount);

    void Save(LoanApplicationViewModel loanApplication, UserAccount userAccount);
}
