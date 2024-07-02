using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectSitesQueryHandler : IRequestHandler<GetProjectSitesQuery, ProjectSitesModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IConsortiumUserContext _userContext;

    private readonly IPrefillDataRepository _prefillDataRepository;

    public GetProjectSitesQueryHandler(IProjectRepository projectRepository, IConsortiumUserContext userContext, IPrefillDataRepository prefillDataRepository)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
        _prefillDataRepository = prefillDataRepository;
    }

    public async Task<ProjectSitesModel> Handle(GetProjectSitesQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var projectSites = await _projectRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);

        var fdProject = await GetProjectPrefillData(request.ProjectId, userAccount, cancellationToken);

        var sites = projectSites.Sites.IsProvided()
            ? projectSites.Sites!
                .TakePage(request.PaginationRequest)
                .Select(x =>
                {
                    var fdSite = fdProject?.Sites?.FirstOrDefault(y => y.Id == x.FdSiteId);

                    return new SiteBasicModel(
                        x.Id.Value,
                        x.Name.Value,
                        request.ProjectId.Value,
                        x.LocalAuthority?.Name ?? fdSite?.LocalAuthorityName,
                        x.Status);
                })
                .ToList()
            : [];

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

    private async Task<ProjectPrefillData?> GetProjectPrefillData(
        FrontDoorProjectId fdProjectId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        ProjectPrefillData? fdProject = null;
        if ((await _userContext.GetSelectedAccount()).CanEdit)
        {
            fdProject = await _prefillDataRepository.GetProjectPrefillData(fdProjectId, userAccount, cancellationToken);
        }

        return fdProject;
    }
}
