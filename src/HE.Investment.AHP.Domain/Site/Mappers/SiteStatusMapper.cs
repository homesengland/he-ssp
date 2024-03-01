using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteStatusMapper : EnumMapper<SiteStatus>
{
    protected override IDictionary<SiteStatus, int?> Mapping =>
        new Dictionary<SiteStatus, int?>
        {
            { SiteStatus.InProgress, (int)invln_Sitestatus.InProgress },
            { SiteStatus.Completed, (int)invln_Sitestatus.Completed },
        };
}
