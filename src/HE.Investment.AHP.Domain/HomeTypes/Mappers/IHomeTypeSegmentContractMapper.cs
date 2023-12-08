using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public interface IHomeTypeSegmentContractMapper<in TSegmentEntity, out TSegment>
    where TSegmentEntity : IHomeTypeSegmentEntity
    where TSegment : HomeTypeSegmentBase
{
    TSegment Map(ApplicationName applicationName, HomeTypeName homeTypeName, TSegmentEntity segment);
}
