using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class SetFrontDoorProjectHandler : CrmActionHandlerBase<invln_setfrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_organisationid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_userid);
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string entityFieldsParameters => ExecutionData.GetInputParameter<string>(invln_setfrontdoorprojectRequest.Fields.invln_entityfieldsparameters);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(organisationId) && !string.IsNullOrEmpty(entityFieldsParameters);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("SetFrontDoorProjectHandler");
            var frontdoorprojectid = CrmServicesFactory.Get<IFrontDoorProjectService>().CreateRecordFromPortal(externalContactId, organisationId, frontDoorProjectId, entityFieldsParameters);
            this.TracingService.Trace("Send Response");
            if (frontdoorprojectid != null)
            {
                ExecutionData.SetOutputParameter(invln_setfrontdoorprojectResponse.Fields.invln_frontdoorprojectid, frontdoorprojectid);
            }
        }
        #endregion
    }
}
