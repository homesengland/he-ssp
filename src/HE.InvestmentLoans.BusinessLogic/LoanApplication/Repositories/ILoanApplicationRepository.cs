using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplicationEntity> GetLoanApplication(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken);

    Task Save(LoanApplicationViewModel loanApplication, UserAccount userAccount);

    Task Save(LoanApplicationEntity loanApplication, UserDetails userDetails, CancellationToken cancellationToken);

    Task WithdrawSubmit(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);

    Task WithdrawDraft(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);
}
