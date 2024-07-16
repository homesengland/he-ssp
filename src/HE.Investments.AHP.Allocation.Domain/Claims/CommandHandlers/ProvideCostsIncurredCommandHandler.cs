using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal sealed class ProvideCostsIncurredCommandHandler : IRequestHandler<ProvideCostsIncurredCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideCostsIncurredCommand request, CancellationToken cancellationToken)
    {
        // TODO: AB#85084 Implement command handler
        if (request.CostsIncurred == null)
        {
            OperationResult.ThrowValidationError(nameof(request.CostsIncurred), "Invalid value");
        }

        return Task.FromResult(OperationResult.Success());
    }
}
