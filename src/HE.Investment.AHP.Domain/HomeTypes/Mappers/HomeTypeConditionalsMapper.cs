using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public static class HomeTypeConditionalsMapper
{
    public static HomeTypeConditionals Map(IHomeTypeEntity homeType)
    {
        return new HomeTypeConditionals(
            homeType.SupportedHousingInformation.ShortStayAccommodation,
            homeType.SupportedHousingInformation.RevenueFundingType,
            homeType.HomeInformation.BuildingType,
            homeType.HomeInformation.AccessibilityStandards,
            homeType.HomeInformation.MeetNationallyDescribedSpaceStandards,
            homeType.TenureDetails.ExemptFromTheRightToSharedOwnership,
            homeType.TenureDetails.IsProspectiveRentIneligible);
    }
}
