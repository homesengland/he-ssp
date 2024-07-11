using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Queries;

public record GetAllocationClaimsQuery(AllocationId AllocationId, PaginationRequest PaginationRequest) : IRequest<AllocationDetails>;
