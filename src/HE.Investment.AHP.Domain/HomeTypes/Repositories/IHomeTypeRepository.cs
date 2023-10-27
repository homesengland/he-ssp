using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypeEntity> GetById(
        string schemeId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken);

    Task<HomeTypeEntity> Save(
        string schemeId,
        HomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken);
}
