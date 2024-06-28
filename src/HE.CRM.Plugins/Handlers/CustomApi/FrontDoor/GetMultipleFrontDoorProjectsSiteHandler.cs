using System;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using FrontDoorProjectSiteV1 = HE.CRM.Plugins.Services.FrontDoorProjectSite;
using FrontDoorProjectSiteV2 = HE.CRM.Plugins.Services.FrontDoorProjectSite.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetMultipleFrontDoorProjectsSiteHandler : CrmActionHandlerBase<invln_getmultiplefrontdoorprojectssiteRequest, DataverseContext>
    {
        #region Fields
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_frontdoorprojectid);
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_fieldstoretrieve);
        private string PagingRequest => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_pagingrequest);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorProjectId) && !string.IsNullOrEmpty(PagingRequest);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(GetMultipleFrontDoorProjectsSiteHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"FieldsToRetrieve: {FieldsToRetrieve}");
            Logger.Trace($"PagingRequest: {PagingRequest}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            var frontDoorProjectIdGuid = Guid.Parse(FrontDoorProjectId);

            var frontDoorProjectSiteDtoList = new PagedResponseDto<FrontDoorProjectSiteDto>();
            if (FeatureFlags.UseNewFrontDoorApiManagement)
            { // New frontdoor apim
                var service = CrmServicesFactory.Get<FrontDoorProjectSiteV2.IFrontDoorProjectSiteService>();

                var paging = JsonSerializer.Deserialize<PagingRequestDto>(PagingRequest, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                frontDoorProjectSiteDtoList = service.GetFrontDoorProjectSites(paging, frontDoorProjectIdGuid, FieldsToRetrieve);
            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectSiteV1.IFrontDoorProjectSiteService>();

                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                var paging = JsonSerializer.Deserialize<PagingRequestDto>(PagingRequest, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                frontDoorProjectSiteDtoList = service.GetFrontDoorProjectSites(paging, FrontDoorProjectId, useHeTables, FieldsToRetrieve);
            }

            this.TracingService.Trace("Send Response");
            if (frontDoorProjectSiteDtoList != null)
            {
                var frontDoorProjectSiteDtoListSerialized = JsonSerializer.Serialize(frontDoorProjectSiteDtoList);
                ExecutionData.SetOutputParameter(invln_getmultiplefrontdoorprojectssiteResponse.Fields.invln_frontdoorprojectsites, frontDoorProjectSiteDtoListSerialized);
            }
        }
        #endregion
    }
}
