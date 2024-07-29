using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public sealed class MilestoneClaimContractMapper : IMilestoneClaimContractMapper
{
    public MilestoneClaim? Map(MilestoneType milestoneType, PhaseEntity phase, DateTime today)
    {
        var milestoneClaim = phase.GetMilestoneClaim(milestoneType);
        if (milestoneClaim.IsNotProvided())
        {
            return null;
        }

        return new MilestoneClaim(
            milestoneType,
            MapStatus(milestoneClaim!.Status, milestoneClaim.CalculateDueStatus(today)),
            milestoneClaim.GrantApportioned.Amount,
            milestoneClaim.GrantApportioned.Percentage,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.ForecastClaimDate)!,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.AchievementDate?.Value),
            milestoneClaim.ClaimDate.SubmissionDate,
            phase.CanMilestoneBeClaimed(milestoneType),
            milestoneClaim.CostsIncurred,
            milestoneClaim.IsConfirmed);
    }

    private static MilestoneStatus MapStatus(Enums.MilestoneStatus status, MilestoneDueStatus dueStatus)
    {
        return status switch
        {
            Enums.MilestoneStatus.Undefined => MapDueStatus(dueStatus),
            Enums.MilestoneStatus.Draft => MapDueStatus(dueStatus),
            Enums.MilestoneStatus.Submitted => MilestoneStatus.Submitted,
            Enums.MilestoneStatus.UnderReview => MilestoneStatus.UnderReview,
            Enums.MilestoneStatus.Approved => MilestoneStatus.Approved,
            Enums.MilestoneStatus.Rejected => MilestoneStatus.Rejected,
            Enums.MilestoneStatus.Paid => MilestoneStatus.Paid,
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
