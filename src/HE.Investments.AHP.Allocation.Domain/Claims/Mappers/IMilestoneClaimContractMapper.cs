using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IMilestoneClaimContractMapper
{
    MilestoneClaim? Map(MilestoneType milestoneType, PhaseEntity phase, DateTime today);
}
