using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract.Requests;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract.Responses;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Mappers;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

public sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetProjectsResponse>(ProjectApiUrls.GetProjects(organisationId), HttpMethod.Get, cancellationToken);

        return response.Select(GetProjectResponseMapper.Map).ToList();
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Support User projects when API is ready
        return await GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await GetProject(projectId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await GetProject(projectId, cancellationToken);
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<CheckProjectExistsRequest, CheckProjectExistsResponse>(
            new CheckProjectExistsRequest(organisationId.ToGuidAsString(), projectName),
            ProjectApiUrls.IsThereProjectWithName,
            HttpMethod.Post,
            cancellationToken);

        return response.Result;
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<SaveProjectRequest, SaveProjectResponse>(
            SaveProjectRequestMapper.Map(dto, organisationId),
            ProjectApiUrls.SaveProject,
            HttpMethod.Post,
            cancellationToken);

        return response.Result;
    }

    private async Task<FrontDoorProjectDto> GetProject(string projectId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<GetProjectResponse>(ProjectApiUrls.GetProject(projectId), HttpMethod.Get, cancellationToken);

        return GetProjectResponseMapper.Map(response);
    }
}
