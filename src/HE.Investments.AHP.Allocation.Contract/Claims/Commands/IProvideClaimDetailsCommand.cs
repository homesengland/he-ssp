using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Commands;

public interface IProvideClaimDetailsCommand : IRequest<OperationResult>
{
    public AllocationId AllocationId { get; }

    public PhaseId PhaseId { get; }

    public MilestoneType MilestoneType { get; }
}
