using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteTypeMapper : EnumMapper<SiteType>
{
    protected override IDictionary<SiteType, int?> Mapping =>
        new Dictionary<SiteType, int?>
        {
            { SiteType.Greenfield, (int)invln_typeofsitesitetable.Greenfield },
            { SiteType.Brownfield, (int)invln_typeofsitesitetable.Brownfield },
            { SiteType.NotApplicable, (int)invln_typeofsitesitetable.NomysiteisnotaBrownfieldorGreenfieldsite },
        };
}
