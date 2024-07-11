using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IClaimsContractMapper
{
    AllocationDetails Map(AllocationEntity allocation, PaginationRequest paginationRequest);

    Phase Map(PhaseEntity phase);

    MilestoneClaim? Map(MilestoneType milestoneType, Domain.Claims.ValueObjects.MilestoneClaim? milestoneClaim);
}
