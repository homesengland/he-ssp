using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetMultipleApplicationsHandler : CrmActionHandlerBase<invln_getmultipleahpapplicationsRequest, DataverseContext>
    {
        #region Fields

        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultipleahpapplicationsRequest.Fields.invln_appfieldstoretrieve);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_getmultipleahpapplicationsRequest.Fields.inlvn_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getmultipleahpapplicationsRequest.Fields.invln_organisationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return contactId != null && organisationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var applications = CrmServicesFactory.Get<IApplicationService>().GetApplication(organisationId, contactId, fieldsToRetrieve);
            if (applications != null)
            {
                var serializedApplications = JsonSerializer.Serialize(applications);
                ExecutionData.SetOutputParameter(invln_getmultipleahpapplicationsResponse.Fields.invln_ahpapplications, serializedApplications);
            }
        }

        #endregion
    }
}
