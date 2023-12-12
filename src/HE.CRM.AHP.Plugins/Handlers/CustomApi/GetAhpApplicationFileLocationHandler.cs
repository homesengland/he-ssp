using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetAhpApplicationFileLocationHandler : CrmActionHandlerBase<invln_getahpapplicationdocumentlocationRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getahpapplicationdocumentlocationRequest.Fields.invln_applicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var documentLocation = CrmServicesFactory.Get<IApplicationService>().GetFileLocationForAhpApplication(applicationId, false);
            ExecutionData.SetOutputParameter(invln_getahpapplicationdocumentlocationResponse.Fields.invln_documentlocation, documentLocation);
        }

        #endregion
    }
}
