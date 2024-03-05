using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<FrontDoorProjectId>>
{
    private readonly IProjectRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public CreateProjectCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<FrontDoorProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            var errorResult = new OperationResult<FrontDoorProjectId>();
            errorResult.AddValidationError("Name", "Enter name");

            return errorResult;
        }

        var project = await _repository.Save(ProjectEntity.New(request.Name), await _accountUserContext.GetSelectedAccount(), cancellationToken);
        return new OperationResult<FrontDoorProjectId>(project.Id);
    }
}
