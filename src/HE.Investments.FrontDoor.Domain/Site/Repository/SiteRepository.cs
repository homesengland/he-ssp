using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public class SiteRepository : ISiteRepository
{
    private readonly ISiteCrmContext _siteCrmContext;

    public SiteRepository(ISiteCrmContext siteCrmContext)
    {
        _siteCrmContext = siteCrmContext;
    }

    public async Task<ProjectSitesEntity> GetSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
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

    private FrontDoorProjectSiteDto ToDto(ProjectSiteEntity entity)
    {
        return new FrontDoorProjectSiteDto
        {
            SiteId = entity.Id.Value,
            SiteName = entity.Name.Value,
        };
    }

    private ProjectSiteEntity ToDomain(FrontDoorProjectSiteDto dto, FrontDoorProjectId projectId)
    {
        return new ProjectSiteEntity(
            new FrontDoorSiteId(dto.SiteId),
            projectId,
            new SiteName(dto.SiteName),
            dto.CreatedOn);
    }
}
