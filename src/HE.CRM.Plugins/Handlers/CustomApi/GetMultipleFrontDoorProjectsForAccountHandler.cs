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
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectsRequest.Fields.invln_fieldstoretrieve);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return organisationId != null;
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetMultipleFrontDoorProjectsForAccountHandler");
            var frontDoorProjectsDtoList = CrmServicesFactory.Get<IFrontDoorProjectService>().GetFrontDoorProjects(organisationId, externalContactId, fieldsToRetrieve);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectsDtoList != null)
            {
                var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDtoList);
                ExecutionData.SetOutputParameter(invln_getmultiplefrontdoorprojectsResponse.Fields.invln_frontdoorprojects, frontDoorProjectDtoListSerialized);
            }
        }
        #endregion
    }
}
