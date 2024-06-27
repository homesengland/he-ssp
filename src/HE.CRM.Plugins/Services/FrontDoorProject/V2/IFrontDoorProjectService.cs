using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Services.FrontDoorProject.V2
{
    public interface IFrontDoorProjectService : ICrmService
    {
        string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters, bool useHeTables);
        List<FrontDoorProjectDto> GetFrontDoorProjects(Guid organisationId, string externalContactId = null);
        FrontDoorProjectDto GetFrontDoorProject(Guid organisationId, string externalContactId, Guid frontDoorProjectId, string includeInactive = null);
        bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, Guid organisationId);
        bool DeactivateFrontDoorProject(string frontDoorProjectId, bool useHeTables);
    }
}
