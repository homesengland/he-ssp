using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Funding.Repositories;
public interface IFundingRepository
{
    Task<FundingEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, FundingFieldsSet fundingFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(FundingEntity funding, UserAccount userAccount, CancellationToken cancellationToken);
}
