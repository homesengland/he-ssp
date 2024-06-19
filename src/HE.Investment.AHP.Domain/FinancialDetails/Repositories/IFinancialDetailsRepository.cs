using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task<FinancialDetailsEntity> GetById(AhpApplicationId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, UserAccount userAccount, CancellationToken cancellationToken);
}
