using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;
using MilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public sealed class MilestoneClaimStatusMapper : IMilestoneClaimStatusMapper
{
    public MilestoneStatus MapStatus(Enums.MilestoneStatus status, MilestoneDueStatus dueStatus)
    {
        return status switch
        {
            Enums.MilestoneStatus.Undefined => MapDueStatus(dueStatus),
            Enums.MilestoneStatus.Draft => MapDueStatus(dueStatus),
            Enums.MilestoneStatus.Submitted => MilestoneStatus.Submitted,
            Enums.MilestoneStatus.UnderReview => MilestoneStatus.UnderReview,
            Enums.MilestoneStatus.Approved => MilestoneStatus.Approved,
            Enums.MilestoneStatus.Rejected => MilestoneStatus.Rejected,
            Enums.MilestoneStatus.Reclaimed => MilestoneStatus.Reclaimed,
            _ => MilestoneStatus.Undefined,
        };
    }

    private static MilestoneStatus MapDueStatus(MilestoneDueStatus dueStatus)
    {
        return dueStatus switch
        {
            MilestoneDueStatus.DueSoon => MilestoneStatus.DueSoon,
            MilestoneDueStatus.Due => MilestoneStatus.Due,
            MilestoneDueStatus.Overdue => MilestoneStatus.Overdue,
            _ => MilestoneStatus.Undefined,
        };
    }
}
