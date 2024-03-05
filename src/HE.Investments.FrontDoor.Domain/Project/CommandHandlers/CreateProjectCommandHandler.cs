using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
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
        var project = await ProjectEntity.New(new ProjectName(request.Name ?? string.Empty), _repository, cancellationToken);
        await _repository.Save(project, await _accountUserContext.GetSelectedAccount(), cancellationToken);
        return new OperationResult<FrontDoorProjectId>(project.Id);
    }
}
