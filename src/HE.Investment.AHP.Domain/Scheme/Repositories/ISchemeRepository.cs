using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investments.Account.Shared.User;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetByApplicationId(AhpApplicationId id, UserAccount userAccount, bool includeFiles, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity entity, UserAccount userAccount, CancellationToken cancellationToken);
}
