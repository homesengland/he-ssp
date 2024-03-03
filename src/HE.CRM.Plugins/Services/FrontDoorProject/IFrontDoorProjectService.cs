using DataverseModel;
using HE.Base.Services;


namespace HE.CRM.Plugins.Services.FrontDoorProject
{
    public interface IFrontDoorProjectService : ICrmService
    {
        string GetFrontDoorProjectsForAccountAndContact(string externalContactId, string accountId, string frontDoorProjectId = null, string fieldsToRetrieve = null);
        string CreateRecordFromPortal(string externalContactId, string organisationId, string frontDoorProjectId, string entityFieldsParameters);
    }
}
