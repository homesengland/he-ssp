using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetByApplicationId(AhpApplicationId id, UserAccount userAccount, bool includeFiles, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity entity, OrganisationId organisationId, CancellationToken cancellationToken);
}
