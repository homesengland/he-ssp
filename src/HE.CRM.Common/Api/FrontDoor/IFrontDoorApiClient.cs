using System;
using System.Threading;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Common.Api.FrontDoor
{
    public interface IFrontDoorApiClient : ICrmService, IDisposable
    {
        bool CheckProjectExists(Guid organisationId, string projectName);

        string DeactivateProject(Guid projectId);

        void RemoveSite(Guid siteId);

        GetProjectsResponse GetProjects(Guid organisationId);

        GetMultipleSitesResponse GetSites(Guid projectId);

        GetProjectResponse GetProject(Guid projectId);

        GetSiteResponse GetSite(Guid siteId);

        SaveProjectResponse SaveProject(FrontDoorProjectDto dto, Guid userId);

        SaveSiteResponse SaveSite(FrontDoorProjectSiteDto dto, Guid projectId);
    }
}
