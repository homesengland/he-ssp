using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Api.Serialization;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

public sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new GetMultipleFrontDoorProjectsRequest { OrganisationId = organisationId.TryToGuidAsString() };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new GetMultipleFrontDoorProjectsRequest
        {
            UserId = userGlobalId,
            OrganisationId = organisationId.TryToGuidAsString(),
        };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
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

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var request = new IsThereProjectWithNameRequest
        {
            ProjectName = projectName,
            OrganisationId = organisationId.TryToGuidAsString(),
        };
        var response = await SendAsync<IsThereProjectWithNameRequest, IsThereProjectWithNameResponse>(
            request,
            ProjectApiUrls.IsThereProjectWithName,
            HttpMethod.Post,
            cancellationToken);

        return response.Exists;
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new SaveFrontDoorProjectRequest
        {
            UserId = userGlobalId,
            OrganisationId = organisationId.ToGuidAsString(),
            ProjectId = dto.ProjectId.IsProvided() ? dto.ProjectId.ToGuidAsString() : string.Empty,
            Project = ApiSerializer.Serialize(dto),
        };
        var response = await SendAsync<SaveFrontDoorProjectRequest, SaveFrontDoorProjectResponse>(
            request,
            ProjectApiUrls.SaveProject,
            HttpMethod.Post,
            cancellationToken);

        return response.ProjectId;
    }

    private async Task<IList<FrontDoorProjectDto>> GetProjects(GetMultipleFrontDoorProjectsRequest request, CancellationToken cancellationToken)
    {
        return await SendAsync<GetMultipleFrontDoorProjectsRequest, GetMultipleFrontDoorProjectsResponse, IList<FrontDoorProjectDto>>(
            request,
            ProjectApiUrls.GetProjects,
            HttpMethod.Post,
            x => string.IsNullOrEmpty(x.Projects) ? "[]" : x.Projects,
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
