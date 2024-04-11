using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Mappers;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class SiteRepository : ISiteRepository
{
    private readonly ISiteCrmContext _siteCrmContext;

    private readonly IPlanningStatusMapper _planningStatusMapper;

    public SiteRepository(ISiteCrmContext siteCrmContext, IPlanningStatusMapper planningStatusMapper)
    {
        _siteCrmContext = siteCrmContext;
        _planningStatusMapper = planningStatusMapper;
    }

    public async Task<bool> IsExist(SiteName name, CancellationToken cancellationToken)
    {
        return await _siteCrmContext.Exist(name.Value, cancellationToken);
    }

    public async Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var paging = new PagingRequestDto { pageNumber = paginationRequest.Page, pageSize = paginationRequest.ItemsPerPage };
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var sites = userAccount.CanViewAllApplications()
            ? await _siteCrmContext.GetOrganisationSites(organisationId, paging, cancellationToken)
            : await _siteCrmContext.GetUserSites(userAccount.UserGlobalId.Value, paging, cancellationToken);

        return new PaginationResult<SiteEntity>(
            sites.items.Select(x => SiteDtoToSiteEntityMapper.Map(x, _planningStatusMapper)).ToList(),
            paginationRequest.Page,
            paginationRequest.ItemsPerPage,
            sites.totalItemsCount);
    }

    public async Task<SiteBasicInfo> GetSiteBasicInfo(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var site = await GetSite(siteId, userAccount, cancellationToken);
        return new SiteBasicInfo(
            site.Id,
            site.Name,
            site.LandAcquisitionStatus,
            site.ModernMethodsOfConstruction.SiteUsingModernMethodsOfConstruction ?? SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes);
    }

    public async Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (siteId.IsNew)
        {
            return SiteEntity.NewSite();
        }

        var site = await _siteCrmContext.GetById(siteId.Value, cancellationToken);

        if (site == null)
        {
            throw new NotFoundException("Site not found", siteId);
        }

        return SiteDtoToSiteEntityMapper.Map(site, _planningStatusMapper);
    }

    public async Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!site.IsModified)
        {
            return site.Id;
        }

        var organisationId = userAccount.SelectedOrganisationId().Value;
        var id = await _siteCrmContext.Save(
            organisationId,
            userAccount.UserGlobalId.Value,
            SiteEntityToSiteDtoMapper.Map(site, _planningStatusMapper),
            cancellationToken);
        return new SiteId(id);
    }
}
