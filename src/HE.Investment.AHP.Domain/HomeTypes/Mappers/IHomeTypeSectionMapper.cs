using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public interface IHomeTypeSectionMapper<out TSection>
{
    public TSection Map(HomeTypeEntity homeType);
}
