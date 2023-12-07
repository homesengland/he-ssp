using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class DisabledPeopleHomeTypeDetailsSegmentContractMapper : IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails>
{
    public DisabledPeopleHomeTypeDetails Map(ApplicationName applicationName, HomeTypeName homeTypeName, DisabledPeopleHomeTypeDetailsSegmentEntity segment)
    {
        return new DisabledPeopleHomeTypeDetails(
            applicationName.Name,
            homeTypeName.Value,
            segment.HousingType,
            segment.ClientGroupType);
    }
}
