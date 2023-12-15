using System;
using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class CheckIfApplicationWithGivenNameExistsHandler : CrmActionHandlerBase<invln_checkifapplicationwithgivennameexistsRequest, DataverseContext>
    {
        #region Fields

        private string application => ExecutionData.GetInputParameter<string>(invln_checkifapplicationwithgivennameexistsRequest.Fields.invln_application);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_checkifapplicationwithgivennameexistsRequest.Fields.invln_organisationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return application != null && organisationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            if (Guid.TryParse(organisationId, out var organisationGuid))
            {
                var isApplicationExists = CrmServicesFactory.Get<IApplicationService>().CheckIfApplicationExists(application, organisationGuid);
                ExecutionData.SetOutputParameter(invln_checkifapplicationwithgivennameexistsResponse.Fields.invln_applicationexists, isApplicationExists.ToString());
            }
        }

        #endregion
    }
}
