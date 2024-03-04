using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProject
{
    public interface IFrontDoorProjectService : ICrmService
    {
        List<FrontDoorProjectDto> GetFrontDoorProjectsForAccountAndContact(string externalContactId, string accountId, string frontDoorProjectId = null, string fieldsToRetrieve = null);
        string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters);
    }
}
