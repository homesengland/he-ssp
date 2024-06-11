using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;

namespace HE.Investments.FrontDoor.Domain.Site.Api;

public sealed class SiteApiContext : ApiHttpClientBase, ISiteContext
{
    public SiteApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        return Task.FromResult(new PagedResponseDto<FrontDoorProjectSiteDto> { items = [], paging = pagination, totalItemsCount = 0, });
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
