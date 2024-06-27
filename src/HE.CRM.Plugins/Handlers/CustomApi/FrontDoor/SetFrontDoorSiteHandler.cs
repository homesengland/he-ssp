using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProjectSite;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProjectSite.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class SetFrontDoorSiteHandler : CrmActionHandlerBase<invln_setfrontdoorsiteRequest, DataverseContext>
    {
        #region Fields
        private string FrontDoorSiteId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_frontdoorsiteid);
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_frontdoorprojectid);
        private string EntityFieldsParameters => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_entityfieldsparameters);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_setfrontdoorsiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorProjectId) && !string.IsNullOrEmpty(EntityFieldsParameters);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(SetFrontDoorSiteHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorSiteId: {FrontDoorSiteId}");
            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"EntityFieldsParameters: {EntityFieldsParameters}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var siteId = string.Empty;
            var frontDoorProjectIdGuid = Guid.Parse(FrontDoorProjectId);

            if (FeatureFlags.UseNewFrontDoorApiManagement && false)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectV2.IFrontDoorProjectSiteService>();

                siteId = service.CreateRecordFromPortal(frontDoorProjectIdGuid, EntityFieldsParameters, FrontDoorSiteId);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectSiteService>();
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                siteId = service.CreateRecordFromPortal(FrontDoorProjectId, EntityFieldsParameters, useHeTables, FrontDoorSiteId);
            }

            this.TracingService.Trace("Send Response");
            if (siteId != null)
            {
                ExecutionData.SetOutputParameter(invln_setfrontdoorsiteResponse.Fields.invln_frontdoorsiteid, siteId);
            }
        }
        #endregion
    }
}
