using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetApplicationHandler : CrmActionHandlerBase<invln_getahpapplicationRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getahpapplicationRequest.Fields.invln_applicationid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getahpapplicationRequest.Fields.invln_appfieldstoretrieve);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_getahpapplicationRequest.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getahpapplicationRequest.Fields.invln_organisationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var application = CrmServicesFactory.Get<IApplicationService>().GetApplication(applicationId, organisationId, contactId, fieldsToRetrieve);
            if (application != null)
            {
                var serializedApplication = JsonSerializer.Serialize(application);
                ExecutionData.SetOutputParameter(invln_getahpapplicationResponse.Fields.invln_retrievedapplicationfields, serializedApplication);
            }
        }

        #endregion
    }
}
