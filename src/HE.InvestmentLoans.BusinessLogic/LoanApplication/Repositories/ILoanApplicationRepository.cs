using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Domain;

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
