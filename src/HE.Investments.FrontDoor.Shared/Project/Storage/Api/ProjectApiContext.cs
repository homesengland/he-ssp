using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api;

internal sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider apiTokenProvider, IApiConfig config)
        : base(httpClient, apiTokenProvider, config, "FrontDoor")
    {
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null)
    {
        var projects = await SendAsync<IList<FrontDoorProjectDto>>(
            CommonProjectApiUrls.Project(organisationId, projectId, includeInactive: includeInactive),
            HttpMethod.Get,
            cancellationToken);

        return projects.FirstOrDefault() ?? throw new NotFoundException("Project", projectId.ToGuidAsString());
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null)
    {
        var projects = await SendAsync<IList<FrontDoorProjectDto>>(
            CommonProjectApiUrls.Project(organisationId, projectId, userGlobalId, includeInactive),
            HttpMethod.Get,
            cancellationToken);

        return projects.FirstOrDefault() ?? throw new NotFoundException("Project", projectId.ToGuidAsString());
    }

    public async Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken)
    {
        return await SendAsync<FrontDoorProjectSiteDto>(CommonProjectApiUrls.Site(projectId, siteId), HttpMethod.Get, cancellationToken);
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken)
    {
        return await SendAsync<PagedResponseDto<FrontDoorProjectSiteDto>>(
            CommonProjectApiUrls.Sites(projectId, pageNumber: 1, pageSize: 100),
            HttpMethod.Get,
            cancellationToken);
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        await SendAsync<DeactivateFrontDoorProjectResponse>(CommonProjectApiUrls.DeleteProject(projectId), HttpMethod.Delete, cancellationToken);
    }
}
