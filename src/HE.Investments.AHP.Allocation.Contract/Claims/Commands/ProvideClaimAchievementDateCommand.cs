using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Commands;

public sealed record ProvideClaimAchievementDateCommand(AllocationId AllocationId, PhaseId PhaseId, MilestoneType MilestoneType, DateDetails? AchievementDate)
    : IProvideClaimDetailsCommand;
