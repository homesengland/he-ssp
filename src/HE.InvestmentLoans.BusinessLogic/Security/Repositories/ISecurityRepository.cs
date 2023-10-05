using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Security.Repositories;
public interface ISecurityRepository
{
    Task<SecurityEntity> GetAsync(LoanApplicationId applicationId, UserAccount userAccount, SecurityFieldsSet securityFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(SecurityEntity entity, UserAccount userAccount, CancellationToken cancellationToken);
}
