using System.Globalization;
using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetails>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetSiteDetailsQueryHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, IProjectRepository projectRepository)
    {
        _siteRepository = siteRepository;
        _accountUserContext = accountUserContext;
        _projectRepository = projectRepository;
    }

    public async Task<SiteDetails> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProject(request.ProjectId, await _accountUserContext.GetSelectedAccount(), cancellationToken);
        var site = await _siteRepository.GetSite(request.ProjectId, request.SiteId, await _accountUserContext.GetSelectedAccount(), cancellationToken);

        return new SiteDetails
        {
            Id = site.Id,
            Name = site.Name.Value,
            ProjectName = project.Name.Value,
            PlanningStatus = site.PlanningStatus.Value,
            LocalAuthorityCode = site.LocalAuthority?.Code.Value,
            LocalAuthorityName = site.LocalAuthority?.Name,
            HomesNumber = site.HomesNumber?.Value.ToString(CultureInfo.InvariantCulture),
        };
    }
}
