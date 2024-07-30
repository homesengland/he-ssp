using HE.Investments.AHP.Allocation.Contract.Claims.Enum;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Commands;

public sealed record SubmitClaimCommand(AllocationId AllocationId, PhaseId PhaseId, MilestoneType MilestoneType) : IProvideClaimDetailsCommand;
