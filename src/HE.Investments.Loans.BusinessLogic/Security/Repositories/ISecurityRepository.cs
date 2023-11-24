using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Security.Repositories;
public interface ISecurityRepository
{
    Task<SecurityEntity> GetAsync(LoanApplicationId applicationId, UserAccount userAccount, SecurityFieldsSet securityFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(SecurityEntity entity, UserAccount userAccount, CancellationToken cancellationToken);
}
