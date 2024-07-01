using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Shared.Project;

internal interface IProjectContext
{
    Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null);

    Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null);

    Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken);

    Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken);

    Task DeactivateProject(string projectId, CancellationToken cancellationToken);
}
