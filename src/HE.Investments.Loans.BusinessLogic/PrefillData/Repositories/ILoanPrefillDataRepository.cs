using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.PrefillData.Entities;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;

public interface ILoanPrefillDataRepository
{
    Task<LoanPrefillData?> GetLoanApplicationPrefillData(LoanApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken);
}
