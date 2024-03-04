using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetails>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetProjectDetailsQueryHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ProjectDetails> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProject(request.ProjectId, await _accountUserContext.GetSelectedAccount(), cancellationToken);

        return new ProjectDetails
        {
            Id = project.Id,
            Name = project.Name,
            IsEnglandHousingDelivery = true,
            IsSiteIdentified = true,
            GeographicFocus = ProjectGeographicFocus.Regional,
            IsFundingRequired = true,
            ActivityTypes = new[] { ActivityType.ProvidingInfrastructure },
        };
    }
}
