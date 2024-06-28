using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProjectSite;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProjectSite.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class DeactivateFrontDoorSiteHandler : CrmActionHandlerBase<invln_deactivatefrontdoorsiteRequest, DataverseContext>
    {
        #region Fields
        private string FrontDoorSiteId => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorsiteRequest.Fields.invln_frontdoorsiteid);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorsiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorSiteId);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(DeactivateFrontDoorSiteHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorSiteId: {FrontDoorSiteId}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var frontDoorSiteIdGuid = Guid.Parse(FrontDoorSiteId);
            var result = false;

            if (FeatureFlags.UseNewFrontDoorApiManagement)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectSiteService>();
                result = service.DeactivateFrontDoorSite(frontDoorSiteIdGuid);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectSiteService>();
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                result = service.DeactivateFrontDoorSite(FrontDoorSiteId, useHeTables);
            }

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_deactivatefrontdoorsiteResponse.Fields.invln_sitedeactivated, result);
        }
        #endregion
    }
}
