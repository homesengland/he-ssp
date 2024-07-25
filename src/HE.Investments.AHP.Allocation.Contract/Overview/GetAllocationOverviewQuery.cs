using MediatR;

namespace HE.Investments.AHP.Allocation.Contract.Overview;

public record GetAllocationOverviewQuery(AllocationId AllocationId) : IRequest<AllocationOverview>;
