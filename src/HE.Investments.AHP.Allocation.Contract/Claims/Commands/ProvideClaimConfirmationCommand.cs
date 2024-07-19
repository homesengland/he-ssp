using HE.Investments.AHP.Allocation.Contract.Claims.Enum;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Commands;

public sealed record ProvideClaimConfirmationCommand(AllocationId AllocationId, PhaseId PhaseId, MilestoneType MilestoneType, bool? IsConfirmed)
    : IProvideClaimDetailsCommand;
