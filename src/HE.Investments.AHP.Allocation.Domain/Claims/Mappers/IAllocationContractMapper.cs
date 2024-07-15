using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IAllocationContractMapper
{
    AllocationDetails Map(AllocationEntity allocation, PaginationRequest paginationRequest);
}
