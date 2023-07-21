using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplicationEntity> Load(LoanApplicationId id, UserAccount userAccount);

    Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken);

    void Save(LoanApplicationViewModel loanApplication, UserAccount userAccount);

    Task Save(LoanApplicationEntity loanApplication, CancellationToken cancellationToken);
}
