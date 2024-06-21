using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

public sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config, "FrontDoor")
    {
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await SendAsync<IList<FrontDoorProjectDto>>(ProjectApiUrls.Projects(organisationId), HttpMethod.Get, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await SendAsync<IList<FrontDoorProjectDto>>(ProjectApiUrls.Projects(organisationId, userGlobalId), HttpMethod.Get, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var projects = await SendAsync<IList<FrontDoorProjectDto>>(
            CommonProjectApiUrls.Project(organisationId, projectId),
            HttpMethod.Get,
            cancellationToken);

        return projects.FirstOrDefault() ?? throw new NotFoundException("Project", projectId);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var projects = await SendAsync<IList<FrontDoorProjectDto>>(
            CommonProjectApiUrls.Project(organisationId, projectId, userGlobalId),
            HttpMethod.Get,
            cancellationToken);

        return projects.FirstOrDefault() ?? throw new NotFoundException("Project", projectId);
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<IsThereProjectWithNameRequest, IsThereProjectWithNameResponse>(
            new IsThereProjectWithNameRequest(projectName),
            ProjectApiUrls.ProjectExists(organisationId),
            HttpMethod.Post,
            cancellationToken);

        return response.ProjectExists;
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<FrontDoorProjectDto, SaveFrontDoorProjectResponse>(
            dto,
            ProjectApiUrls.Project(organisationId, dto.ProjectId),
            string.IsNullOrEmpty(dto.ProjectId) ? HttpMethod.Post : HttpMethod.Put,
            cancellationToken);

        return response.ProjectId;
    }
}
