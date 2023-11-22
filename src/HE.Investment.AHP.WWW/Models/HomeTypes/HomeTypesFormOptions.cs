using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class HomeTypesFormOptions
{
    public static IEnumerable<SelectListItem> FinishHomeTypes => SelectListHelper.FromEnum<FinishHomeTypesAnswer>();

    public static IEnumerable<SelectListItem> HousingTypes => SelectListHelper.FromEnum<HousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleHousingTypes => SelectListHelper.FromEnum<DisabledPeopleHousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleClientGroupTypes => SelectListHelper.FromEnum<DisabledPeopleClientGroupType>();

    public static IEnumerable<SelectListItem> OlderPeopleHousingTypes => SelectListHelper.FromEnum<OlderPeopleHousingType>();

    public static IEnumerable<SelectListItem> HappiDesignPrinciplesExceptNone => SelectListHelper.FromEnum<HappiDesignPrincipleType>()
        .Where(x => x.Value != HappiDesignPrincipleType.NoneOfThese.ToString());

    public static IEnumerable<SelectListItem> HappiDesignPrinciplesOnlyNone => new[] { SelectListHelper.FromEnum(HappiDesignPrincipleType.NoneOfThese) };

    public static IEnumerable<SelectListItem> RevenueFundingTypes => SelectListHelper.FromEnum<RevenueFundingType>();
}
