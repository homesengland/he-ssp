using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class HomeTypesFormOptions
{
    public static IEnumerable<SelectListItem> HousingTypes => SelectListHelper.FromEnum<HousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleHousingTypes => SelectListHelper.FromEnum<DisabledPeopleHousingType>();

    public static IEnumerable<SelectListItem> DisabledPeopleClientGroupTypes => SelectListHelper.FromEnum<DisabledPeopleClientGroupType>();

    public static IEnumerable<SelectListItem> OlderPeopleHousingTypes => SelectListHelper.FromEnum<OlderPeopleHousingType>();
}
