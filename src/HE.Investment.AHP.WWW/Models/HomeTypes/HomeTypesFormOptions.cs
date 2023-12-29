using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class HomeTypesFormOptions
{
    public static IEnumerable<SelectListItem> RemoveHomeTypes => SelectListHelper.FromEnum<RemoveHomeTypeAnswer>();

    public static IEnumerable<SelectListItem> FinishHomeTypes => SelectListHelper.FromEnum<FinishHomeTypesAnswer>();

    public static IEnumerable<SelectListItem> HousingTypes => SelectListHelper.FromEnum<HousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleHousingTypes => SelectListHelper.FromEnum<DisabledPeopleHousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleClientGroupTypes => SelectListHelper.FromEnum<DisabledPeopleClientGroupType>();

    public static IEnumerable<SelectListItem> OlderPeopleHousingTypes => SelectListHelper.FromEnum<OlderPeopleHousingType>();

    public static IEnumerable<SelectListItem> HappiDesignPrinciplesExceptNone => SelectListHelper.FromEnum<HappiDesignPrincipleType>()
        .Where(x => x.Value != HappiDesignPrincipleType.NoneOfThese.ToString());

    public static IEnumerable<SelectListItem> HappiDesignPrinciplesOnlyNone => new[] { SelectListHelper.FromEnum(HappiDesignPrincipleType.NoneOfThese) };

    public static IEnumerable<SelectListItem> RevenueFundingTypes => SelectListHelper.FromEnum<RevenueFundingType>();

    public static IEnumerable<SelectListItem> RevenueFundingSourceTypes => SelectListHelper.FromEnum<RevenueFundingSourceType>();

    public static IEnumerable<SelectListItem> BuildingType => SelectListHelper.FromEnum<BuildingType>();

    public static IEnumerable<SelectListItem> FacilityType => SelectListHelper.FromEnum<FacilityType>();

    public static IEnumerable<SelectListItem> AccessibilityCategoryType => SelectListHelper.FromEnum<AccessibilityCategoryType>();

    public static IEnumerable<SelectListItem> NationallyDescribedSpaceStandardsExceptNone => SelectListHelper.FromEnum<NationallyDescribedSpaceStandardType>()
        .Where(x => x.Value != NationallyDescribedSpaceStandardType.NoneOfThese.ToString());

    public static IEnumerable<SelectListItem> NationallyDescribedSpaceStandardsOnlyNone => new[] { SelectListHelper.FromEnum(NationallyDescribedSpaceStandardType.NoneOfThese) };
}
