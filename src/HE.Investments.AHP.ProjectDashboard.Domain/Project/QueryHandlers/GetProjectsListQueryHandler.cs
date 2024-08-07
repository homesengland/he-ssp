using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Queries;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.QueryHandlers;

public class GetProjectsListQueryHandler : IRequestHandler<GetProjectsListQuery, ProjectsListModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IConsortiumUserContext _userContext;

    private readonly IMediator _mediator;

    private readonly IProgrammeSettings _programmeSettings;

    public GetProjectsListQueryHandler(
        IProjectRepository projectRepository,
        IConsortiumUserContext userContext,
        IMediator mediator,
        IProgrammeSettings programmeSettings)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
        _mediator = mediator;
        _programmeSettings = programmeSettings;
    }

    public async Task<ProjectsListModel> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var ahpProjects = await _projectRepository.GetProjects(request.PaginationRequest, userAccount, cancellationToken);
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(_programmeSettings.AhpProgrammeId)), cancellationToken);

        var projectModels = ahpProjects.Items
            .Select(p => new ProjectModel(
                p.Id,
                p.Name.Value,
                p.Sites?.Select(s => new SiteProjectModel(s.Id, s.Name, s.Status)).ToList()))
            .ToList();

        return new ProjectsListModel(
            userAccount.Organisation!.RegisteredCompanyName,
            programme.Name,
            new PaginationResult<ProjectModel>(
                projectModels,
                ahpProjects.CurrentPage,
                ahpProjects.ItemsPerPage,
                ahpProjects.TotalItems));
    }
}
