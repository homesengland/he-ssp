using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme;

public interface ISchemeRepository
{
    Task<SchemeEntity> GetById(SchemeId schemeId, CancellationToken cancellationToken);

    Task<IList<SchemeEntity>> GetAll(CancellationToken cancellationToken);

    Task<SchemeEntity> Save(SchemeEntity scheme, CancellationToken cancellationToken);
}
