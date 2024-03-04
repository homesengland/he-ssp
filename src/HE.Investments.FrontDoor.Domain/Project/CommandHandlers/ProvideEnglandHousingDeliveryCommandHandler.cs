using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideEnglandHousingDeliveryCommandHandler : IRequestHandler<ProvideEnglandHousingDeliveryCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideEnglandHousingDeliveryCommand request, CancellationToken cancellationToken)
    {
        // TODO: move to Domain
        if (request.IsEnglandHousingDelivery == null)
        {
            return Task.FromResult(OperationResult.New().AddValidationError("IsEnglandHousingDelivery", "Select"));
        }

        return Task.FromResult(OperationResult.Success());
    }
}
