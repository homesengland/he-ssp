using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetAbsoluteAhpApplicationFileLocationHandler : CrmActionHandlerBase<invln_getabsoluteahpapplicationfilelocationRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getabsoluteahpapplicationfilelocationRequest.Fields.invln_applicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var documentLocation = CrmServicesFactory.Get<IApplicationService>().GetFileLocationForAhpApplication(applicationId, true);
            ExecutionData.SetOutputParameter(invln_getabsoluteahpapplicationfilelocationResponse.Fields.invln_documentlocation, documentLocation);
        }

        #endregion
    }
}
