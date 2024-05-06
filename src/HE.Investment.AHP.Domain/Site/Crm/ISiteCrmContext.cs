using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Site.Crm;

public interface ISiteCrmContext
{
    Task<PagedResponseDto<SiteDto>> GetOrganisationSites(string organisationId, PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<PagedResponseDto<SiteDto>> GetUserSites(string userGlobalId, PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<SiteDto?> GetById(string siteId, CancellationToken cancellationToken);

    Task<bool> Exist(string name, CancellationToken cancellationToken);

    Task<bool> StrategicSiteExist(string name, string organisationId, CancellationToken cancellationToken);

    Task<string> Save(string organisationId, string userGlobalId, SiteDto dto, CancellationToken cancellationToken);
}
