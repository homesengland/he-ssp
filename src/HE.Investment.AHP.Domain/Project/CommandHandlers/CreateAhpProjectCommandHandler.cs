using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.CommandHandlers;

public class CreateAhpProjectCommandHandler : IRequestHandler<CreateAhpProjectCommand, AhpProjectId>
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    private readonly IProjectRepository _projectRepository;

    private readonly IAhpUserContext _ahpUserContext;

    public CreateAhpProjectCommandHandler(IPrefillDataRepository prefillDataRepository, IProjectRepository projectRepository, IAhpUserContext ahpUserContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _projectRepository = projectRepository;
        _ahpUserContext = ahpUserContext;
    }

    public async Task<AhpProjectId> Handle(CreateAhpProjectCommand request, CancellationToken cancellationToken)
    {
        var account = await _ahpUserContext.GetSelectedAccount();
        var frontDoorProject = await _prefillDataRepository.GetProjectPrefillData(request.FrontDoorProjectId, account, cancellationToken);

        var ahpProject = await _projectRepository.CreateProject(frontDoorProject, account, cancellationToken);

        return ahpProject;
    }
}
