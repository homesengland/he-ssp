using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Models.App;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

public sealed class SiteCrmApiHttpClient : CrmApiHttpClient, ISiteCrmContext
{
    public SiteCrmApiHttpClient(HttpClient httpClient, ICrmApiTokenProvider tokenProvider, ICrmApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }
}
