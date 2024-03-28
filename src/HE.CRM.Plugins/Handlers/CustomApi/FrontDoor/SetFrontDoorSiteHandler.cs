using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProjectSite;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class SetFrontDoorSiteHandler : CrmActionHandlerBase<invln_setfrontdoorsiteRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorSiteId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_frontdoorsiteid);
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_frontdoorprojectid);
        private string entityFieldsParameters => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_entityfieldsparameters);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectId) && !string.IsNullOrEmpty(entityFieldsParameters);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("SetFrontDoorSiteHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var siteId = CrmServicesFactory.Get<IFrontDoorProjectSiteService>().CreateRecordFromPortal(frontDoorProjectId, entityFieldsParameters, useHeTables, frontDoorSiteId);
            this.TracingService.Trace("Send Response");
            if (siteId != null)
            {
                ExecutionData.SetOutputParameter(invln_setfrontdoorsiteResponse.Fields.invln_frontdoorsiteid, siteId);
            }
        }
        #endregion
    }
}
