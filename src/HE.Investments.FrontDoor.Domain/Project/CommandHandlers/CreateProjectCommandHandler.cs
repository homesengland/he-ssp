using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<FrontDoorProjectId>>
{
    private readonly IProjectRepository _repository;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public CreateProjectCommandHandler(IProjectRepository repository, IConsortiumUserContext consortiumUserContext)
    {
        _repository = repository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<OperationResult<FrontDoorProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        var projectNameExist = new ProjectNameWithinOrganisationExists(_repository, userAccount);
        var project = await ProjectEntity.New(new ProjectName(request.Name ?? string.Empty), projectNameExist, !userAccount.Consortium.HasNoConsortium, cancellationToken);

        await _repository.Save(project, userAccount, cancellationToken);
        return new OperationResult<FrontDoorProjectId>(project.Id);
    }
}
