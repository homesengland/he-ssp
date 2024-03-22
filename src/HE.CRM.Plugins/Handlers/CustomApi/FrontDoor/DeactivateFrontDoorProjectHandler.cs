using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class DeactivateFrontDoorProjectHandler : CrmActionHandlerBase<invln_deactivatefrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglefrontdoorprojectRequest.Fields.invln_usehetables);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("DeactivateFrontDoorProjectHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var result = CrmServicesFactory.Get<IFrontDoorProjectService>().DeactivateFrontDoorProject(frontDoorProjectId, useHeTables);
            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_deactivatefrontdoorprojectResponse.Fields.invln_projectdeactivated, result);
        }
        #endregion
    }
}
