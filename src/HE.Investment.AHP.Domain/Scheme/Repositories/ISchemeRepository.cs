using HE.Investment.AHP.Domain.Scheme.Entities;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity?> GetById(DomainApplicationId id, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(DomainApplicationId id, SchemeEntity entity, CancellationToken cancellationToken);
}
