using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract.Requests;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract.Responses;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Mappers;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api;

public sealed class SiteApiContext : ApiHttpClientBase, ISiteContext
{
    public SiteApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(
        string projectId,
        UserAccount userAccount,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetMultipleSitesResponse>(SiteApiUrls.GetSites(projectId), HttpMethod.Get, cancellationToken);
        var sites = response
            .TakePage(new PaginationRequest(pagination.pageNumber, pagination.pageSize))
            .Select(GetSiteResponseMapper.Map)
            .ToList();

        return new PagedResponseDto<FrontDoorProjectSiteDto>
        {
            items = sites,
            paging = pagination,
            totalItemsCount = response.Count,
        };
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

    public async Task Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new RemoveSiteRequest(siteId.ToGuidAsString());
        await SendAsync<RemoveSiteRequest, RemoveSiteResponse>(request, SiteApiUrls.RemoveSite, HttpMethod.Post, cancellationToken);
    }
}
