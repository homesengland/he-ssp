using HE.Investments.AHP.Allocation.Contract.Claims.Enum;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IMilestoneClaimStatusMapper
{
    MilestoneStatus MapStatus(Enums.MilestoneStatus status, DateTime forecastClaimDate);
}
