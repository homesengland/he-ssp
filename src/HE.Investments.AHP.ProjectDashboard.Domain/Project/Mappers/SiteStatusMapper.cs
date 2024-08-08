using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Mappers;

internal sealed class SiteStatusMapper : EnumMapper<SiteStatus>
{
    protected override SiteStatus? ToDomainMissing => SiteStatus.InProgress;

    protected override IDictionary<SiteStatus, int?> Mapping =>
        new Dictionary<SiteStatus, int?>
        {
            { SiteStatus.InProgress, (int)invln_Sitestatus.InProgress },
            { SiteStatus.Submitted, (int)invln_Sitestatus.Submitted },
        };
}
