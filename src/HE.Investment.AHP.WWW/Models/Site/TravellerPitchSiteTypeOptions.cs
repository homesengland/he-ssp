using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Site;

public static class TravellerPitchSiteTypeOptions
{
    private static readonly Dictionary<TravellerPitchSiteType, string> Types = new()
    {
        {
            TravellerPitchSiteType.Permanent,
            "Sites that are intended for permanent use as a traveller pitch site and provide pitches for long-term use by residents."
        },
        {
            TravellerPitchSiteType.Transit,
            "Sites that are intended for the permanent provision of transit pitches, providing temporary accommodation for travellers for up to 3 months."
        },
        {
            TravellerPitchSiteType.Temporary,
            "Sites that are only intended for temporary use as a traveller pitch site or which lack planning approval for permanent provision of traveller pitches."
        },
    };

    public static IEnumerable<ExtendedSelectListItem> TravellerPitchSiteTypes => Types.ToExtendedSelectList();
}
