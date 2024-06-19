using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Api.Serialization;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

internal sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider apiTokenProvider, IApiConfig config)
        : base(httpClient, apiTokenProvider, config)
    {
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new GetSingleFrontDoorProjectRequest
        {
            OrganisationId = organisationId.TryToGuidAsString(),
            ProjectId = projectId.ToGuidAsString(),
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new GetSingleFrontDoorProjectRequest
        {
            OrganisationId = organisationId.TryToGuidAsString(),
            UserId = userGlobalId,
            ProjectId = projectId.ToGuidAsString(),
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken)
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

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken)
    {
        var request = new GetMultipleFrontDoorSitesRequest
        {
            ProjectId = projectId.ToGuidAsString(),
            PagingRequest = ApiSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 100 }),
        };

        return await SendAsync<
            GetMultipleFrontDoorSitesRequest,
            GetMultipleFrontDoorSitesResponse,
            PagedResponseDto<FrontDoorProjectSiteDto>>(
            request,
            CommonProjectApiUrls.GetSites,
            HttpMethod.Post,
            x => x.Sites,
            cancellationToken);
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        var request = new DeactivateFrontDoorProjectRequest
        {
            ProjectId = projectId.ToGuidAsString(),
        };

        await SendAsync<DeactivateFrontDoorProjectRequest, DeactivateFrontDoorProjectResponse>(
            request,
            CommonProjectApiUrls.DeactivateProject,
            HttpMethod.Post,
            cancellationToken);
    }

    private async Task<FrontDoorProjectDto> GetProject(GetSingleFrontDoorProjectRequest request, CancellationToken cancellationToken)
    {
        var projects = await SendAsync<GetSingleFrontDoorProjectRequest, GetSingleFrontDoorProjectResponse, IList<FrontDoorProjectDto>>(
            request,
            CommonProjectApiUrls.GetProject,
            HttpMethod.Post,
            x => string.IsNullOrEmpty(x.Projects) ? "[]" : x.Projects,
            cancellationToken);

        return projects.FirstOrDefault() ?? throw new NotFoundException("Project", request.ProjectId);
    }
}
