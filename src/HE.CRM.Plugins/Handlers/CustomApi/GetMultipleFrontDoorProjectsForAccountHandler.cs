using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;


namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetMultipleFrontDoorProjectsForAccountHandler : CrmActionHandlerBase<invln_getsingleloanapplicationforaccountandcontactRequest, DataverseContext>
    {
        #region Fields
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.invln_organisationid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.inlvn_userid);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(organisationId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetMultipleFrontDoorProjectsForAccountHandler");
            var frontDoorProjectsDto = CrmServicesFactory.Get<IFrontDoorProjectService>().GetFrontDoorProjectsForAccountAndContact(externalContactId, organisationId);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectsDto != null)
            {
                var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDto);
                ExecutionData.SetOutputParameter(invln_getmultiplefrontdoorprojectsResponse.Fields.invln_frontdoorprojects, frontDoorProjectDtoListSerialized);
            }
        }
        #endregion
    }
}
