using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class DeleteLoanApplicationHandler : CrmActionHandlerBase<invln_deleteloanapplicationRequest, DataverseContext>
    {
        #region Fields

        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_deleteloanapplicationRequest.Fields.invln_loanapplicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return loanApplicationId != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().DeleteLoanApplication(loanApplicationId);
        }

        #endregion
    }
}
