using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;

public interface ILoanApplicationRepository : ILoansFileLocationProvider<SupportingDocumentsParams>
{
    Task<LoanApplicationEntity> GetLoanApplication(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(LoanApplicationName loanApplicationName, UserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken);

    Task Save(LoanApplicationEntity loanApplication, UserProfileDetails userDetails, CancellationToken cancellationToken);

    Task WithdrawSubmitted(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);

    Task WithdrawDraft(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken);

    Task MoveToDraft(LoanApplicationId loanApplicationId, CancellationToken cancellationToken);

    Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken);
}
