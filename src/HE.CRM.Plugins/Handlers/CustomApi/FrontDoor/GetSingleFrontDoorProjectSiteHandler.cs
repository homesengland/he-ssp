using System;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using FrontDoorProjectSiteV1 = HE.CRM.Plugins.Services.FrontDoorProjectSite;
using FrontDoorProjectSiteV2 = HE.CRM.Plugins.Services.FrontDoorProjectSite.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetSingleFrontDoorProjectSiteHandler : CrmActionHandlerBase<invln_getsinglefrontdoorprojectsiteRequest, DataverseContext>
    {
        #region Fields
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_frontdoorprojectid);
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_fieldstoretrieve);
        private string FrontDoorProjectSiteId => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_frontdoorprojectsiteid);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectsiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorProjectSiteId);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(GetSingleFrontDoorProjectSiteHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"FieldsToRetrieve: {FieldsToRetrieve}");
            Logger.Trace($"FrontDoorProjectSiteId: {FrontDoorProjectSiteId}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var frontDoorProjectIdGuid = Guid.Parse(FrontDoorProjectId);
            var frontDoorProjectSiteIdGuid = Guid.Parse(FrontDoorProjectSiteId);

            var frontDoorProjectSiteDto = new FrontDoorProjectSiteDto();

            if (FeatureFlags.UseNewFrontDoorApiManagement && false)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectSiteV2.IFrontDoorProjectSiteService>();
                frontDoorProjectSiteDto = service.GetFrontDoorProjectSite(frontDoorProjectIdGuid, frontDoorProjectSiteIdGuid);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectSiteV1.IFrontDoorProjectSiteService>();
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                frontDoorProjectSiteDto = service.GetFrontDoorProjectSite(FrontDoorProjectId, useHeTables, FieldsToRetrieve, FrontDoorProjectSiteId);
            }

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
