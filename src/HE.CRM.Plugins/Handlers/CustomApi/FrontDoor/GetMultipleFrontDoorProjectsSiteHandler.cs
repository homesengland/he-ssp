using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProjectSite;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class GetMultipleFrontDoorProjectsSiteHandler : CrmActionHandlerBase<invln_getmultiplefrontdoorprojectssiteRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_frontdoorprojectid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_fieldstoretrieve);
        private string pagingRequest => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_pagingrequest);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectId) && !string.IsNullOrEmpty(pagingRequest);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetMultipleFrontDoorProjectsSiteHandler");
            var paging = JsonSerializer.Deserialize<PagingRequestDto>(pagingRequest);
            var frontDoorProjectSiteDtoList = CrmServicesFactory.Get<IFrontDoorProjectSiteService>().GetFrontDoorProjectSites(paging, frontDoorProjectId, fieldsToRetrieve);
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
