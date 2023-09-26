using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class ChangeLoanApplicationExternalStatusHandler : CrmActionHandlerBase<invln_changeloanapplicationexternalstatusRequest, DataverseContext>
    {
        #region Fields

        private int externalStatus => ExecutionData.GetInputParameter<int>(invln_changeloanapplicationexternalstatusRequest.Fields.invln_statusexternal);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_changeloanapplicationexternalstatusRequest.Fields.invln_loanapplicationid);
        private string changeReason => ExecutionData.GetInputParameter<string>(invln_changeloanapplicationexternalstatusRequest.Fields.invln_changereason);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return externalStatus != null && !string.IsNullOrEmpty(loanApplicationId);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().ChangeLoanApplicationExternalStatus(externalStatus, loanApplicationId, changeReason);
        }

        #endregion
    }
}
