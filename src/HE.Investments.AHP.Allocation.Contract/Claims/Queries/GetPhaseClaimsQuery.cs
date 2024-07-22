using MediatR;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Queries;

public record GetPhaseClaimsQuery(AllocationId AllocationId, PhaseId PhaseId) : IRequest<Phase>;
