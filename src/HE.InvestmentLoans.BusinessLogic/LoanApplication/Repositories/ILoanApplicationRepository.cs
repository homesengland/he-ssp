using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplicationEntity> GetLoanApplication(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(LoanApplicationName loanApplicationName, UserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken);

    Task Save(LoanApplicationEntity loanApplication, UserDetails userDetails, CancellationToken cancellationToken);

    Task WithdrawSubmitted(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);

    Task WithdrawDraft(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);

    Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken);
}
