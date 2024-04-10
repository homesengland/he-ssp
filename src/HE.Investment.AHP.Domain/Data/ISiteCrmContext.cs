using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Data;

public interface ISiteCrmContext
{
    Task<PagedResponseDto<SiteDto>> GetOrganisationSites(Guid organisationId, PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<PagedResponseDto<SiteDto>> GetUserSites(string userGlobalId, PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<SiteDto?> GetById(string siteId, CancellationToken cancellationToken);

    Task<bool> Exist(string name, CancellationToken cancellationToken);

    Task<string> Save(Guid organisationId, string userGlobalId, SiteDto dto, CancellationToken cancellationToken);
}
