using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.Mappers;

public interface IHomeTypeSectionMapper<out TSection>
{
    public TSection Map(HomeTypeEntity homeType);
}
