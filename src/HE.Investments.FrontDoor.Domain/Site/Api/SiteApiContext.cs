using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.FrontDoor.Domain.Project.Api.Contract.Responses;
using HE.Investments.FrontDoor.Domain.Site.Api.Contract.Requests;
using HE.Investments.FrontDoor.Domain.Site.Api.Contract.Responses;
using HE.Investments.FrontDoor.Domain.Site.Api.Mappers;

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

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetSiteResponse>(SiteApiUrls.GetSite(siteId), HttpMethod.Get, cancellationToken);

        return GetSiteResponseMapper.Map(response);
    }

    public async Task<string> Save(
        string projectId,
        FrontDoorProjectSiteDto dto,
        string userGlobalId,
        string organisationId,
        CancellationToken cancellationToken)
    {
        var request = SaveSiteRequestMapper.Map(dto, projectId);
        var response = await SendAsync<SaveSiteRequest, SaveSiteResponse>(request, SiteApiUrls.SaveSite, HttpMethod.Post, cancellationToken);

        return response.Result;
    }

    public Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }
}
