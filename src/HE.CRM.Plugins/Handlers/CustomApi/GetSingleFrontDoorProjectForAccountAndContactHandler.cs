using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;


namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetSingleFrontDoorProjectForAccountAndContactHandler : CrmActionHandlerBase<invln_getsingleloanapplicationforaccountandcontactRequest, DataverseContext>
    {
        #region Fields
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_organisationid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_userid);
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_fieldstoretrieve);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(organisationId) && !string.IsNullOrEmpty(frontDoorProjectId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleFrontDoorProjectForAccountAndContact");
            var frontDoorProjectDto = CrmServicesFactory.Get<IFrontDoorProjectService>().GetFrontDoorProjectsForAccountAndContact(externalContactId, organisationId, frontDoorProjectId, fieldsToRetrieve);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectDto != null)
            {
                var frontDoorProjectDtoSerialized = JsonSerializer.Serialize(frontDoorProjectDto);
                ExecutionData.SetOutputParameter(invln_getsinglefrontdoorprojectResponse.Fields.invln_retrievedfrontdoorprojectfields, frontDoorProjectDtoSerialized);
            }
        }
        #endregion
    }
}



