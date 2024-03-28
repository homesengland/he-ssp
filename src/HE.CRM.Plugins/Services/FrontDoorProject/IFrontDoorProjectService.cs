using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProject
{
    public interface IFrontDoorProjectService : ICrmService
    {
        string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters, bool useHeTables);
        List<FrontDoorProjectDto> GetFrontDoorProjects(string organisationId, bool useHeTables, string externalContactId = null, string fieldsToRetrieve = null, string frontDoorProjectId = null, string includeInactive = null);
        bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, bool useHeTables, string organisationId = null);
        bool DeactivateFrontDoorProject(string frontDoorProjectId, bool useHeTables);
    }
}
