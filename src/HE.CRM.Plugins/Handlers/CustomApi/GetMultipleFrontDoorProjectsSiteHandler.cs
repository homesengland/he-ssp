using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProjectSite;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    internal class GetMultipleFrontDoorProjectsSiteHandler : CrmActionHandlerBase<invln_getmultiplefrontdoorprojectssiteRequest, DataverseContext>
    {
        #region Fields
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_organisationid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_externalcontactid);
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_getmultiplefrontdoorprojectssiteRequest.Fields.invln_frontdoorprojectid);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(organisationId) && !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(frontDoorProjectId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetMultipleFrontDoorProjectsSiteHandler");
            var frontDoorProjectSiteDtoList = CrmServicesFactory.Get<IFrontDoorProjectSiteService>().GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(frontDoorProjectId, organisationId, externalContactId);
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
