using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
public interface IFundingRepository
{
    Task<FundingEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, FundingFieldsSet fundingFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(FundingEntity funding, UserAccount userAccount, CancellationToken cancellationToken);
}
