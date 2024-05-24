using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectsListQueryHandler : IRequestHandler<GetProjectsListQuery, ProjectsListModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAhpUserContext _userContext;

    private readonly IAhpProgrammeRepository _programmeRepository;

    public GetProjectsListQueryHandler(
        IProjectRepository projectRepository,
        IAhpUserContext userContext,
        IAhpProgrammeRepository programmeRepository)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
        _programmeRepository = programmeRepository;
    }

    public async Task<ProjectsListModel> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var ahpProjects = await _projectRepository.GetProjects(request.PaginationRequest, userAccount, cancellationToken);

        var projectModels = ahpProjects.Items
            .Select(p => new ProjectModel(
                p.Id,
                p.Name.Value,
                p.Sites?.Select(s => new SiteProjectModel(s.Id, s.Name.Value, s.Status)).ToList()))
            .ToList();

        return new ProjectsListModel(
            userAccount.Organisation!.RegisteredCompanyName,
            (await _programmeRepository.GetProgramme(cancellationToken)).Name,
            new PaginationResult<ProjectModel>(
                projectModels,
                ahpProjects.CurrentPage,
                ahpProjects.ItemsPerPage,
                ahpProjects.TotalItems));
    }
}
