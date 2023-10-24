using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes;

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
