using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetByApplicationId(AhpApplicationId id, ConsortiumUserAccount userAccount, bool includeFiles, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity entity, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
