using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.CommandHandlers;

public class CreateAhpProjectIfDoesNotExistCommandHandler : IRequestHandler<CreateAhpProjectIfDoesNotExistCommand, AhpProjectId>
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    private readonly IProjectRepository _projectRepository;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public CreateAhpProjectIfDoesNotExistCommandHandler(IPrefillDataRepository prefillDataRepository, IProjectRepository projectRepository, IConsortiumUserContext consortiumUserContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _projectRepository = projectRepository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<AhpProjectId> Handle(CreateAhpProjectIfDoesNotExistCommand request, CancellationToken cancellationToken)
    {
        var account = await _consortiumUserContext.GetSelectedAccount();

        var ahpProject = await _projectRepository.GetProjectOrNull(request.FrontDoorProjectId, account, cancellationToken);
        if (ahpProject != null)
        {
            return AhpProjectId.From(ahpProject.AhpProjectId);
        }

        var frontDoorProject = await _prefillDataRepository.GetProjectPrefillData(request.FrontDoorProjectId, account, cancellationToken);
        var ahpProjectId = await _projectRepository.CreateProject(frontDoorProject, account, cancellationToken);
        return ahpProjectId;
    }
}
