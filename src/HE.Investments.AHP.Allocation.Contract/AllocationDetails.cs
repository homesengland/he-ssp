using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.Allocation.Contract;

public record AllocationDetails(AllocationBasicInfo AllocationBasicInfo, GrantDetails GrantDetails, PaginationResult<Phase> PhaseList);
