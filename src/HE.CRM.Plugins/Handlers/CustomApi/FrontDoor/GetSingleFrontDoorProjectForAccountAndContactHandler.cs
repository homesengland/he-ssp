using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;


namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSingleFrontDoorProjectForAccountAndContactHandler : CrmActionHandlerBase<invln_getsinglefrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_organisationid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_userid);
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_fieldstoretrieve);
        private string includeInactive => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_includeinactive);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return organisationId != null;
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleFrontDoorProjectForAccountAndContact");
            var frontDoorProjectsDtoList = CrmServicesFactory.Get<IFrontDoorProjectService>().GetFrontDoorProjects(organisationId, externalContactId, fieldsToRetrieve, frontDoorProjectId, includeInactive);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectsDtoList != null)
            {
                var frontDoorProjectDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectsDtoList);
                ExecutionData.SetOutputParameter(invln_getsinglefrontdoorprojectResponse.Fields.invln_retrievedfrontdoorprojectfields, frontDoorProjectDtoListSerialized);
            }
        }
        #endregion
    }
}



