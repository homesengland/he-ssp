using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class OlderPeopleHomeTypeDetailsSegmentContractMapper : IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails>
{
    public OlderPeopleHomeTypeDetails Map(ApplicationName applicationName, HomeTypeName homeTypeName, OlderPeopleHomeTypeDetailsSegmentEntity segment)
    {
        return new OlderPeopleHomeTypeDetails(
            applicationName.Value,
            homeTypeName.Value,
            segment.HousingType);
    }
}
