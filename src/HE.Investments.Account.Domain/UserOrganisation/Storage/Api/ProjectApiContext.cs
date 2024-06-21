using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Api;

public sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config, "FrontDoor")
    {
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string organisationId, CancellationToken cancellationToken)
    {
        return await SendAsync<IList<FrontDoorProjectDto>>(ProjectApiUrls.Projects(organisationId), HttpMethod.Get, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await SendAsync<IList<FrontDoorProjectDto>>(ProjectApiUrls.Projects(organisationId, userGlobalId), HttpMethod.Get, cancellationToken);
    }
}
