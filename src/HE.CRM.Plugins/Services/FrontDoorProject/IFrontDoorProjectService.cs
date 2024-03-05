using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProject
{
    public interface IFrontDoorProjectService : ICrmService
    {
        string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters);
        List<FrontDoorProjectDto> GetFrontDoorProjects(string organisationId, string externalContactId = null, string fieldsToRetrieve = null, string frontDoorProjectId = null);
    }
}
