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

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return application != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IApplicationService>().SetApplication(application, fieldsToUpdate);
        }

        #endregion
    }
}
