using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class SupportedHousingInformationSegmentContractMapper : IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation>
{
    public SupportedHousingInformation Map(ApplicationName applicationName, HomeTypeName homeTypeName, SupportedHousingInformationSegmentEntity segment)
    {
        return new SupportedHousingInformation(
            applicationName.Name,
            homeTypeName.Value,
            segment.LocalCommissioningBodiesConsulted,
            segment.ShortStayAccommodation,
            segment.RevenueFundingType,
            segment.RevenueFundingSources.ToList(),
            segment.MoveOnArrangements?.Value,
            segment.TypologyLocationAndDesign?.Value,
            segment.ExitPlan?.Value);
    }
}
