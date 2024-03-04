using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideProjectNameCommandHandler : IRequestHandler<ProvideProjectNameCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideProjectNameCommand request, CancellationToken cancellationToken)
    {
        // TODO: move to Domain
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Task.FromResult(OperationResult.New().AddValidationError("Name", "Enter name"));
        }

        return Task.FromResult(OperationResult.Success());
    }
}
