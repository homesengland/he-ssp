using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProjectSite;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSingleFrontDoorProjectSiteHandler : CrmActionHandlerBase<invln_getsinglefrontdoorprojectsiteRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_frontdoorprojectid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_fieldstoretrieve);
        private string frontDoorProjectSiteId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_frontdoorprojectsiteid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectSiteId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleFrontDoorProjectSiteHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var frontDoorProjectSiteDto = CrmServicesFactory.Get<IFrontDoorProjectSiteService>().GetFrontDoorProjectSite(frontDoorProjectId, useHeTables, fieldsToRetrieve, frontDoorProjectSiteId);
            this.TracingService.Trace("Send Response");
            if (frontDoorProjectSiteDto != null)
            {
                var frontDoorProjectSiteDtoSerialized = JsonSerializer.Serialize(frontDoorProjectSiteDto);
                ExecutionData.SetOutputParameter(invln_getsinglefrontdoorprojectsiteResponse.Fields.invln_frontdoorprojectsite, frontDoorProjectSiteDtoSerialized);
            }
        }
        #endregion
    }
}
