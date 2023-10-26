using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

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
