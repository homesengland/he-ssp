using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class HomeTypesFormOptions
{
    private static readonly IEnumerable<HousingType> AvailableHousingTypes =
        new[] { HousingType.General, HousingType.HousingForOlderPeople, HousingType.HousingForDisabledAndVulnerablePeople };

    public static IList<SelectListItem> HousingTypes { get; } =
        AvailableHousingTypes.Select(x => SelectListHelper.FromEnum(x, GetHousingTypeDescription(x))).ToList();

    public static string GetHousingTypeDescription(HousingType housingType)
    {
        return housingType switch
        {
            HousingType.Undefined => string.Empty,
            HousingType.General => "General",
            HousingType.HousingForOlderPeople => "Housing for older people",
            HousingType.HousingForDisabledAndVulnerablePeople => "Housing for disabled and vulnerable people",
            _ => throw new ArgumentOutOfRangeException(nameof(housingType), housingType, null),
        };
    }
}
