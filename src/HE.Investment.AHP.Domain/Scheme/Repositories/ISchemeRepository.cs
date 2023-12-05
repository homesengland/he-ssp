using HE.Investment.AHP.Domain.Scheme.Entities;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetByApplicationId(DomainApplicationId id, bool includeFiles, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity entity, CancellationToken cancellationToken);
}
