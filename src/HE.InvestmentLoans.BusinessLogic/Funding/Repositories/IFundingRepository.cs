using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
public interface IFundingRepository
{
    Task<FundingEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, FundingViewOption fundingViewOption, CancellationToken cancellationToken);

    Task SaveAsync(FundingEntity funding, UserAccount userAccount, CancellationToken cancellationToken);
}
