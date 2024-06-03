using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectSitesQueryHandler : IRequestHandler<GetProjectSitesQuery, ProjectSitesModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IConsortiumUserContext _userContext;

    public GetProjectSitesQueryHandler(IProjectRepository projectRepository, IConsortiumUserContext userContext)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
    }

    public async Task<ProjectSitesModel> Handle(GetProjectSitesQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var projectSites = await _projectRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);
        var sites = projectSites.Sites.IsProvided() ? projectSites.Sites!
            .TakePage(request.PaginationRequest)
            .Select(x => new SiteBasicModel(
                x.Id.Value,
                x.Name.Value,
                request.ProjectId.Value,
                x.LocalAuthority?.Name,
                x.Status))
            .ToList() : [];

        return new ProjectSitesModel(
            projectSites.Id,
            projectSites.Name.Value,
            userAccount.Organisation!.RegisteredCompanyName,
            new PaginationResult<SiteBasicModel>(
                sites,
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                projectSites.Sites?.Count ?? 0));
    }
}
