using HE.Investment.AHP.Domain.Scheme.Entities;
using DomainSchemeId = HE.Investment.AHP.Domain.Scheme.ValueObjects.SchemeId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetById(DomainSchemeId id, CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity entity, CancellationToken cancellationToken);
}
