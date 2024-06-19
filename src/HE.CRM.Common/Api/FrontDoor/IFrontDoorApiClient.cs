using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Api.FrontDoor
{
    public interface IFrontDoorApiClient
    {
        Task<bool> CheckProjectExists(Guid organisationId, string projectName, CancellationToken cancellationToken);

        Task DeactivateProject(Guid projectId, CancellationToken cancellationToken);

        Task RemoveSite(Guid siteId, CancellationToken cancellationToken);

        Task<IList<FrontDoorProjectDto>> GetProjects(Guid organisationId, CancellationToken cancellationToken);

        Task<IList<FrontDoorProjectSiteDto>> GetSites(Guid projectId, CancellationToken cancellationToken);

        Task<FrontDoorProjectDto> GetProject(Guid projectId, CancellationToken cancellationToken);

        Task<FrontDoorProjectSiteDto> GetSite(Guid siteId, CancellationToken cancellationToken);

        Task<Guid> SaveProject(FrontDoorProjectDto dto, Guid userId, CancellationToken cancellationToken);

        Task<Guid> SaveSite(FrontDoorProjectSiteDto dto, Guid projectId, CancellationToken cancellationToken);
    }
}
