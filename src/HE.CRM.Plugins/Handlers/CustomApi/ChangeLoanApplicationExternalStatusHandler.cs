using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using HE.CRM.Plugins.Services.RichTextService;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class ChangeLoanApplicationExternalStatusHandler : CrmActionHandlerBase<invln_changeloanapplicationexternalstatusRequest, DataverseContext>
    {
        #region Fields

        private string externalStatus => ExecutionData.GetInputParameter<string>(invln_changeloanapplicationexternalstatusRequest.Fields.invln_externalstatus);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_changeloanapplicationexternalstatusRequest.Fields.invln_loanapplicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalStatus) && !string.IsNullOrEmpty(loanApplicationId);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().ChangeLoanApplicationExternalStatus(externalStatus, loanApplicationId);
        }

        #endregion
    }
}
