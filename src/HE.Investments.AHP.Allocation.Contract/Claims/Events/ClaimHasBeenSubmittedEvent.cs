using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Events;

public record ClaimHasBeenSubmittedEvent(AllocationId AllocationId, MilestoneType MilestoneType) : IDomainEvent;
