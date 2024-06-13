using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract.Requests;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract.Responses;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Mappers;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

internal sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider apiTokenProvider, IApiConfig config)
        : base(httpClient, apiTokenProvider, config)
    {
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken)
    {
        return await GetProject(projectId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Support User project when API is ready
        return await GetProject(projectId, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetSiteResponse>(CommonProjectApiUrls.GetSite(siteId), HttpMethod.Get, cancellationToken);

        return GetSiteResponseMapper.Map(response);
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetMultipleSitesResponse>(CommonProjectApiUrls.GetSites(projectId), HttpMethod.Get, cancellationToken);
        var sites = response.Select(GetSiteResponseMapper.Map).ToList();

        return new PagedResponseDto<FrontDoorProjectSiteDto>
        {
            items = sites,
            paging = new PagingRequestDto { pageNumber = 1, pageSize = 100 },
            totalItemsCount = response.Count,
        };
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        var request = new DeactivateProjectRequest(projectId.ToGuidAsString());
        await SendAsync<DeactivateProjectRequest, DeactivateProjectResponse>(request, CommonProjectApiUrls.DeactivateProject, HttpMethod.Post, cancellationToken);
    }

    private async Task<FrontDoorProjectDto> GetProject(string projectId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetProjectResponse>(CommonProjectApiUrls.GetProject(projectId), HttpMethod.Get, cancellationToken);

        return GetProjectResponseMapper.Map(response);
    }
}
