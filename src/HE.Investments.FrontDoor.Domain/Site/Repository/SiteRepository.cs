extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using SiteLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public class SiteRepository : ISiteRepository, IRemoveSiteRepository
{
    private readonly ISiteCrmContext _siteCrmContext;

    private readonly PlanningStatusMapper _planningStatusMapper = new();

    public SiteRepository(ISiteCrmContext siteCrmContext)
    {
        _siteCrmContext = siteCrmContext;
    }

    public async Task<ProjectSitesEntity> GetProjectSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var sites = await _siteCrmContext.GetSites(projectId.Value, userAccount, new PagingRequestDto { pageNumber = 1, pageSize = 100 }, cancellationToken);

        return new ProjectSitesEntity(projectId, sites.items.Select(x => ToDomain(x, projectId)).ToList());
    }

    public async Task<ProjectSiteEntity> GetSite(FrontDoorProjectId projectId, FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var site = await _siteCrmContext.GetSite(projectId.Value, siteId.Value, userAccount, cancellationToken);

        return ToDomain(site, projectId);
    }

    public async Task<ProjectSiteEntity> Save(ProjectSiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var siteId = await _siteCrmContext.Save(site.ProjectId.Value, ToDto(site), userAccount, cancellationToken);
        if (site.Id.IsNew)
        {
            site.SetId(new FrontDoorSiteId(siteId));
        }

        return site;
    }

    public async Task<string> Remove(FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return await _siteCrmContext.Remove(siteId.Value, userAccount, cancellationToken);
    }

    private FrontDoorProjectSiteDto ToDto(ProjectSiteEntity entity)
    {
        return new FrontDoorProjectSiteDto
        {
            SiteId = entity.Id.Value,
            SiteName = entity.Name.Value,
            NumberofHomesEnabledBuilt = entity.HomesNumber?.Value,
            PlanningStatus = entity.PlanningStatus.Value == SitePlanningStatus.Undefined ? null : _planningStatusMapper.ToDto(entity.PlanningStatus.Value),
            LocalAuthorityCode = entity.LocalAuthority?.Code.Value,
            LocalAuthorityName = entity.LocalAuthority?.Name,
        };
    }

    private ProjectSiteEntity ToDomain(FrontDoorProjectSiteDto dto, FrontDoorProjectId projectId)
    {
        return new ProjectSiteEntity(
            projectId,
            new FrontDoorSiteId(dto.SiteId),
            new SiteName(dto.SiteName),
            dto.CreatedOn,
            dto.NumberofHomesEnabledBuilt == null ? null : new HomesNumber(dto.NumberofHomesEnabledBuilt.Value),
            planningStatus: PlanningStatus.Create(_planningStatusMapper.ToDomain(dto.PlanningStatus)),
            localAuthority: string.IsNullOrWhiteSpace(dto.LocalAuthorityCode) ? null : SiteLocalAuthority.New(dto.LocalAuthorityCode, dto.LocalAuthorityName));
    }
}
