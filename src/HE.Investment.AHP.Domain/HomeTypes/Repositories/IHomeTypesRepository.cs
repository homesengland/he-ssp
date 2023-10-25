using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypesRepository
{
    Task<HomeTypesEntity> GetBySchemeId(string schemeId, CancellationToken cancellationToken);
}
