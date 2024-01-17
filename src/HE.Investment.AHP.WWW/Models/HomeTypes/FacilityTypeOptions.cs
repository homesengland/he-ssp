using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public static class FacilityTypeOptions
{
    private static readonly Dictionary<FacilityType, string> Types = new()
    {
        { FacilityType.SelfContainedFacilities, "Resident has use of their own facilities, such as bathroom and kitchen, within their own home." },
        { FacilityType.SharedFacilities, "Residents have their own room or rooms but facilities, such as bathroom and kitchen, are shared with others." },
        { FacilityType.MixOfSelfContainedAndSharedFacilities, string.Empty },
    };

    public static IEnumerable<ExtendedSelectListItem> FacilityTypes => Types.ToExtendedSelectList();
}
