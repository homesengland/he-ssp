using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Commands;

public sealed record ProvideCostsIncurredCommand(AllocationId AllocationId, PhaseId PhaseId, MilestoneType MilestoneType, bool? CostsIncurred) : IRequest<OperationResult>, IProvideClaimDetailsCommand;
