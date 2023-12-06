using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class HomeInformationSegmentContractMapper : IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation>
{
    public HomeInformation Map(ApplicationName applicationName, HomeTypeName homeTypeName, HomeInformationSegmentEntity segment)
    {
        return new HomeInformation(
            applicationName.Name,
            homeTypeName.Value,
            segment.NumberOfHomes?.Value,
            segment.NumberOfBedrooms?.Value,
            segment.MaximumOccupancy?.Value,
            segment.NumberOfStoreys?.Value,
            segment.IntendedAsMoveOnAccommodation,
            segment.PeopleGroupForSpecificDesignFeatures,
            segment.BuildingType,
            segment.CustomBuild,
            segment.FacilityType,
            segment.AccessibilityStandards,
            segment.AccessibilityCategory);
    }
}
