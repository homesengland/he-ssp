using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;

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
        return await SendAsync<PagedResponseDto<FrontDoorProjectSiteDto>>(
            CommonProjectApiUrls.Sites(projectId, pagination.pageNumber, pagination.pageSize),
            HttpMethod.Get,
            cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return await SendAsync<FrontDoorProjectSiteDto>(CommonProjectApiUrls.Site(projectId, siteId), HttpMethod.Get, cancellationToken);
    }

    public async Task<string> Save(
        string projectId,
        FrontDoorProjectSiteDto dto,
        string userGlobalId,
        string organisationId,
        CancellationToken cancellationToken)
    {
        var response = await SendAsync<FrontDoorProjectSiteDto, SaveFrontDoorSiteResponse>(
            dto,
            SiteApiUrls.Site(projectId, dto.SiteId),
            string.IsNullOrEmpty(dto.SiteId) ? HttpMethod.Post : HttpMethod.Put,
            cancellationToken);

        return response.SiteId;
    }

    public async Task Remove(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        await SendAsync<RemoveFrontDoorSiteResponse>(SiteApiUrls.RemoveSite(projectId, siteId), HttpMethod.Delete, cancellationToken);
    }
}
