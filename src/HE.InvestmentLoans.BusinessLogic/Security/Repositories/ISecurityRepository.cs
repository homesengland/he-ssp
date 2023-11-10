using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.Security.Repositories;
public interface ISecurityRepository
{
    Task<SecurityEntity> GetAsync(LoanApplicationId applicationId, UserAccount userAccount, SecurityFieldsSet securityFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(SecurityEntity entity, UserAccount userAccount, CancellationToken cancellationToken);
}
