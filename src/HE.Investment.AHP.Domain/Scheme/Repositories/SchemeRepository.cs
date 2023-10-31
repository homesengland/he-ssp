using HE.Investment.AHP.Domain.Mock;
using HE.Investment.AHP.Domain.Scheme.Entities;
using DomainSchemeId = HE.Investment.AHP.Domain.Scheme.ValueObjects.SchemeId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : InMemoryRepository<SchemeEntity>, ISchemeRepository
{
    public async Task<SchemeEntity> GetById(DomainSchemeId id, CancellationToken cancellationToken)
    {
        return await GetById(id.Value, cancellationToken);
    }

    public async Task<SchemeEntity> Save(SchemeEntity entity, CancellationToken cancellationToken)
    {
        return await Save(entity.Id.Value, entity, cancellationToken);
    }
}
