using HE.Investment.AHP.Domain.Mock;
using HE.Investment.AHP.Domain.Scheme.Entities;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : InMemoryRepository<SchemeEntity>, ISchemeRepository
{
    public async Task<SchemeEntity?> GetById(DomainApplicationId id, CancellationToken cancellationToken)
    {
        return await GetById(id.Value, cancellationToken);
    }

    public async Task<SchemeEntity> Save(DomainApplicationId id, SchemeEntity entity, CancellationToken cancellationToken)
    {
        return await Save(id.Value, entity, cancellationToken);
    }
}
