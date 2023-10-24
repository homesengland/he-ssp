using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes;

public interface IHomeTypeRepository
{
    Task<HomeTypeEntity> Get(
        HomeTypeId homeTypeId,
        HomeTypeSectionType sectionTypes,
        CancellationToken cancellationToken);

    Task<HomeTypeEntity> Save(
        string financialSchemeId,
        HomeTypeEntity homeType,
        HomeTypeSectionType sectionTypes,
        CancellationToken cancellationToken);
}
