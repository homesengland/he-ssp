using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.Mappers;

public class HousingTypeSectionMapper : IHomeTypeSectionMapper<HousingTypeSection>
{
    public HousingTypeSection Map(HomeTypeEntity homeType)
    {
        return new HousingTypeSection(homeType.Id!.Value)
        {
            HomeTypeName = homeType.Name?.Value,
            HousingType = homeType.HousingTypeSectionEntity.HousingType,
        };
    }
}
