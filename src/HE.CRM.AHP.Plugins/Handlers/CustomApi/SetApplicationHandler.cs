using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SetApplicationHandler : CrmActionHandlerBase<invln_setahpapplicationRequest, DataverseContext>
    {
        #region Fields

        private string application => ExecutionData.GetInputParameter<string>(invln_setahpapplicationRequest.Fields.invln_application);
        private string fieldsToUpdate => ExecutionData.GetInputParameter<string>(invln_setahpapplicationRequest.Fields.invln_fieldstoupdate);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_setahpapplicationRequest.Fields.invln_organisationid);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_setahpapplicationRequest.Fields.invln_userid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return application != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var recordGuid = CrmServicesFactory.Get<IApplicationService>().SetApplication(application, organisationId, contactId, fieldsToUpdate);
            ExecutionData.SetOutputParameter(invln_setahpapplicationResponse.Fields.invln_applicationid, recordGuid.ToString());
        }

        #endregion
    }
}
