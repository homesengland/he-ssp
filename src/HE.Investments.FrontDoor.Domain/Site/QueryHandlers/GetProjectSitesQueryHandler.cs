using System.Globalization;
using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.QueryHandlers;

public class GetProjectSitesQueryHandler : IRequestHandler<GetProjectSitesQuery, ProjectSites>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetProjectSitesQueryHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    {
        _siteRepository = siteRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ProjectSites> Handle(GetProjectSitesQuery request, CancellationToken cancellationToken)
    {
        var projectSitesEntity = await _siteRepository.GetProjectSites(request.ProjectId, await _accountUserContext.GetSelectedAccount(), cancellationToken);

        return new ProjectSites
        {
            ProjectId = request.ProjectId,
            Sites = projectSitesEntity.Sites.Select(x => new SiteDetails
            {
                Id = x.Id,
                Name = x.Name.Value,
                PlanningStatus = x.PlanningStatus.Value,
                LocalAuthorityCode = x.LocalAuthority?.Id.Value,
                LocalAuthorityName = x.LocalAuthority?.Name,
                HomesNumber = x.HomesNumber?.Value.ToString(CultureInfo.InvariantCulture),
            }).ToList(),
        };
    }
}
