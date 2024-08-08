using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Commands;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.CommandHandlers;

public class TryCreateAhpProjectCommandHandler : IRequestHandler<TryCreateAhpProjectCommand, AhpProjectId>
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    private readonly IProjectRepository _projectRepository;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public TryCreateAhpProjectCommandHandler(IPrefillDataRepository prefillDataRepository, IProjectRepository projectRepository, IConsortiumUserContext consortiumUserContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _projectRepository = projectRepository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<AhpProjectId> Handle(TryCreateAhpProjectCommand request, CancellationToken cancellationToken)
    {
        var account = await _consortiumUserContext.GetSelectedAccount();

        var ahpProject = await _projectRepository.TryGetProject(request.FrontDoorProjectId, account, cancellationToken);
        if (ahpProject != null)
        {
            return AhpProjectId.From(ahpProject.AhpProjectId);
        }

        var frontDoorProject = await _prefillDataRepository.GetProjectPrefillData(request.FrontDoorProjectId, account, cancellationToken);
        var ahpProjectId = await _projectRepository.CreateProject(frontDoorProject, account, cancellationToken);
        return ahpProjectId;
    }
}
