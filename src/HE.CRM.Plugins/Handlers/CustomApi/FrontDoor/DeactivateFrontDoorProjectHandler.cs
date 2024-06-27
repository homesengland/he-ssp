using DataverseModel;
using HE.Base.Plugins.Handlers;
using FrontDoorProjectV1 = HE.CRM.Plugins.Services.FrontDoorProject;
using FrontDoorProjectV2 = HE.CRM.Plugins.Services.FrontDoorProject.V2;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class DeactivateFrontDoorProjectHandler : CrmActionHandlerBase<invln_deactivatefrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string FrontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string UseHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(FrontDoorProjectId);
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(DeactivateFrontDoorProjectHandler)}.{nameof(DoWork)}");

            Logger.Trace($"FrontDoorProjectId: {FrontDoorProjectId}");
            Logger.Trace($"UseHeTablesFromPortal: {UseHeTablesFromPortal}");

            if (FeatureFlags.UseNewFrontDoorApiManagement && false)
            { // New frontdoor apim

            }
            else
            { // old version
                var service = CrmServicesFactory.Get<FrontDoorProjectV1.IFrontDoorProjectService>();
                var useHeTables = !string.IsNullOrEmpty(UseHeTablesFromPortal);
                var result = service.DeactivateFrontDoorProject(FrontDoorProjectId, useHeTables);
                this.TracingService.Trace("Send Response");
                ExecutionData.SetOutputParameter(invln_deactivatefrontdoorprojectResponse.Fields.invln_projectdeactivated, result);
            }
        }
        #endregion
    }
}
