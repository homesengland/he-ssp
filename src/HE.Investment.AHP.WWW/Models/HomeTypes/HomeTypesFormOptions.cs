using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class HomeTypesFormOptions
{
    public static List<SelectListItem> HousingTypes { get; } = new()
    {
        SelectListHelper.FromEnum(HousingType.General, "General"),
        SelectListHelper.FromEnum(HousingType.HousingForOlderPeople, "Housing for older people"),
        SelectListHelper.FromEnum(HousingType.HousingForDisabledAndVulnerablePeople, "Housing for disabled and vulnerable people"),
    };
}
