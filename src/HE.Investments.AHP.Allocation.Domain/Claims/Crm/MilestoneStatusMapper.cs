using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public sealed class MilestoneStatusMapper : EnumMapper<MilestoneStatus>
{
    protected override MilestoneStatus? ToDomainMissing => MilestoneStatus.Draft;

    protected override IDictionary<MilestoneStatus, int?> Mapping =>
        new Dictionary<MilestoneStatus, int?>
        {
            { MilestoneStatus.Draft, (int)invln_ClaimExternalStatus.Draft },
            { MilestoneStatus.Submitted, (int)invln_ClaimExternalStatus.Submitted },
            { MilestoneStatus.UnderReview, (int)invln_ClaimExternalStatus.UnderReview },
            { MilestoneStatus.Approved, (int)invln_ClaimExternalStatus.Approved },
            { MilestoneStatus.Rejected, (int)invln_ClaimExternalStatus.Rejected },
            { MilestoneStatus.Paid, (int)invln_ClaimExternalStatus.Paid },
        };
}
