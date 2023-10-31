using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public interface IHomeTypeSegmentMapper<out TSegment>
{
    public TSegment Map(HomeTypeEntity homeType);
}
