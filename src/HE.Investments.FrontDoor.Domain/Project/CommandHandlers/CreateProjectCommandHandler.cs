using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<FrontDoorProjectId>>
{
    public Task<OperationResult<FrontDoorProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // TODO: move to Domain
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            var errorResult = new OperationResult<FrontDoorProjectId>();
            errorResult.AddValidationError("Name", "Enter name");

            return Task.FromResult(errorResult);
        }

        var result = new OperationResult<FrontDoorProjectId>(new FrontDoorProjectId(Guid.NewGuid().ToString()));
        return Task.FromResult(result);
    }
}
