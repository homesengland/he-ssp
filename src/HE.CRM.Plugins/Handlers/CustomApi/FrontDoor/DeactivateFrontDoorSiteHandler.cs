using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProjectSite;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class DeactivateFrontDoorSiteHandler : CrmActionHandlerBase<invln_deactivatefrontdoorsiteRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorSiteId => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorsiteRequest.Fields.invln_frontdoorsiteid);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorSiteId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("DeactivateFrontDoorSiteHandler");
            var result = CrmServicesFactory.Get<IFrontDoorProjectSiteService>().DeactivateFrontDoorSite(frontDoorSiteId);
            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_deactivatefrontdoorsiteResponse.Fields.invln_sitedeactivated, result);
        }
        #endregion
    }
}
