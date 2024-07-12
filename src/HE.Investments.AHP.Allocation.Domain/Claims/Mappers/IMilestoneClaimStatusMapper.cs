using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using MilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IMilestoneClaimStatusMapper
{
    MilestoneStatus MapStatus(Enums.MilestoneStatus status, MilestoneDueStatus dueStatus);
}
