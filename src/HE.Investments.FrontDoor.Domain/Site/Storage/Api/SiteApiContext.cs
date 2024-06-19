using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Api.Serialization;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

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
        var request = new GetMultipleFrontDoorSitesRequest
        {
            ProjectId = projectId.ToGuidAsString(),
            PagingRequest = ApiSerializer.Serialize(pagination),
        };

        return await SendAsync<GetMultipleFrontDoorSitesRequest, GetMultipleFrontDoorSitesResponse, PagedResponseDto<FrontDoorProjectSiteDto>>(
                request,
                CommonProjectApiUrls.GetSites,
                HttpMethod.Post,
                x => x.Sites,
                cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new GetSingleFrontDoorSiteRequest
        {
            ProjectId = projectId.ToGuidAsString(),
            SiteId = siteId.ToGuidAsString(),
        };

        return await SendAsync<GetSingleFrontDoorSiteRequest, GetSingleFrontDoorSiteResponse, FrontDoorProjectSiteDto>(
            request,
            CommonProjectApiUrls.GetSite,
            HttpMethod.Post,
            x => x.Site,
            cancellationToken);
    }

    public async Task<string> Save(
        string projectId,
        FrontDoorProjectSiteDto dto,
        string userGlobalId,
        string organisationId,
        CancellationToken cancellationToken)
    {
        var request = new SaveFrontDoorSiteRequest
        {
            ProjectId = projectId.ToGuidAsString(),
            SiteId = dto.SiteId.IsProvided() ? dto.SiteId.ToGuidAsString() : string.Empty,
            Site = ApiSerializer.Serialize(dto),
        };
        var response = await SendAsync<SaveFrontDoorSiteRequest, SaveFrontDoorSiteResponse>(
            request,
            SiteApiUrls.SaveSite,
            HttpMethod.Post,
            cancellationToken);

        return response.SiteId;
    }

    public async Task Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new RemoveFrontDoorSiteRequest { SiteId = siteId.ToGuidAsString() };

        await SendAsync<RemoveFrontDoorSiteRequest, RemoveFrontDoorSiteResponse>(
            request,
            SiteApiUrls.RemoveSite,
            HttpMethod.Post,
            cancellationToken);
    }
}
