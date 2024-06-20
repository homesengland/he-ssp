using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Domain.UserOrganisation.Storage.Api.Contract;
using HE.Investments.Api;
using HE.Investments.Api.Auth;
using HE.Investments.Api.Config;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Api;

public sealed class ProjectApiContext : ApiHttpClientBase, IProjectContext
{
    public ProjectApiContext(HttpClient httpClient, IApiTokenProvider tokenProvider, IApiConfig config)
        : base(httpClient, tokenProvider, config)
    {
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string organisationId, CancellationToken cancellationToken)
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

    private async Task<IList<FrontDoorProjectDto>> GetProjects(GetMultipleFrontDoorProjectsRequest request, CancellationToken cancellationToken)
    {
        return await SendAsync<GetMultipleFrontDoorProjectsRequest, GetMultipleFrontDoorProjectsResponse, IList<FrontDoorProjectDto>>(
            request,
            "getProjects",
            HttpMethod.Post,
            x => string.IsNullOrEmpty(x.Projects) ? "[]" : x.Projects,
            cancellationToken);
    }
}
