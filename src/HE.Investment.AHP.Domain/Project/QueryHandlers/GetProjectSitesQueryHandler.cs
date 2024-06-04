using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.PrefillData.Repositories;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectSitesQueryHandler : IRequestHandler<GetProjectSitesQuery, ProjectSitesModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAhpPrefillDataRepository _prefillDataRepository;

    private readonly IConsortiumUserContext _userContext;

    public GetProjectSitesQueryHandler(IProjectRepository projectRepository, IAhpPrefillDataRepository prefillDataRepository, IConsortiumUserContext userContext)
    {
        _projectRepository = projectRepository;
        _prefillDataRepository = prefillDataRepository;
        _userContext = userContext;
    }

    public async Task<ProjectSitesModel> Handle(GetProjectSitesQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var projectSites = await _projectRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);
        List<SiteBasicModel> sites = [];

        if (projectSites.Sites.IsProvided())
        {
            foreach (var site in projectSites.Sites!.TakePage(request.PaginationRequest))
            {
                var localAuthorityName = string.IsNullOrEmpty(site.LocalAuthority?.Name)
                    ? (await _prefillDataRepository.GetAhpSitePrefillData(request.ProjectId, site.FdSiteId, cancellationToken)).LocalAuthorityName!
                    : site.LocalAuthority.Name;

                var siteModel = new SiteBasicModel(
                    site.Id.Value,
                    site.Name.Value,
                    request.ProjectId.Value,
                    localAuthorityName,
                    site.Status);

                sites.Add(siteModel);
            }
        }

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
