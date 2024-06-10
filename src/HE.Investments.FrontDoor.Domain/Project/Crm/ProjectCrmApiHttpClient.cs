using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Models.App;
using HE.Investments.FrontDoor.Domain.Project.Crm.Requests;
using HE.Investments.FrontDoor.Domain.Project.Crm.Responses;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public sealed class ProjectCrmApiHttpClient : CrmApiHttpClient, IProjectCrmContext
{
    public ProjectCrmApiHttpClient(HttpClient httpClient, ICrmApiTokenProvider tokenProvider, ICrmApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var response = await SendAsync<CheckProjectExistsRequest, CheckProjectExistsResponse>(
            new CheckProjectExistsRequest(organisationId.ToGuidAsString(), projectName),
            "CheckProjectExists",
            HttpMethod.Post,
            cancellationToken);

        return response.Result;
    }

    public Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        // TODO: AB#98936 Implement API connection
        throw new NotImplementedException();
    }
}
