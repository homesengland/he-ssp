using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class TravellerPitchSiteTypeMapper : EnumMapper<TravellerPitchSiteType>
{
    protected override IDictionary<TravellerPitchSiteType, int?> Mapping =>
        new Dictionary<TravellerPitchSiteType, int?>
        {
            { TravellerPitchSiteType.Permanent, (int)invln_Travellerpitchsitetype.Permament },
            { TravellerPitchSiteType.Transit, (int)invln_Travellerpitchsitetype.Transit },
            { TravellerPitchSiteType.Temporary, (int)invln_Travellerpitchsitetype.Temporary },
            { TravellerPitchSiteType.Undefined, null },
        };

    protected override TravellerPitchSiteType? ToDomainMissing => TravellerPitchSiteType.Undefined;
}
