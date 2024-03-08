using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.FrontDoorProject;

namespace HE.CRM.Plugins.Handlers.CustomApi.FrontDoor
{
    public class DeactivateFrontDoorProjectHandler : CrmActionHandlerBase<invln_deactivatefrontdoorprojectRequest, DataverseContext>
    {
        #region Fields
        private string frontDoorProjectId => ExecutionData.GetInputParameter<string>(invln_deactivatefrontdoorprojectRequest.Fields.invln_frontdoorprojectid);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(frontDoorProjectId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("DeactivateFrontDoorProjectHandler");
            var result = CrmServicesFactory.Get<IFrontDoorProjectService>().DeactivateFrontDoorProject(frontDoorProjectId);
            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_deactivatefrontdoorprojectResponse.Fields.invln_projectdeactivated, result);
        }
        #endregion
    }
}
