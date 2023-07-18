using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class UpdateSingleLoanApplicationHandler : CrmActionHandlerBase<invln_updatesingleloanapplicationRequest, DataverseContext>
    {
        #region Fields

        private string loanApplication => ExecutionData.GetInputParameter<string>(invln_updatesingleloanapplicationRequest.Fields.invln_loanapplication);
        private string fieldsToUpdate => ExecutionData.GetInputParameter<string>(invln_updatesingleloanapplicationRequest.Fields.invln_fieldstoupdate);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_updatesingleloanapplicationRequest.Fields.invln_loanapplicationid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_updatesingleloanapplicationRequest.Fields.invln_accountid);
        private string contactExternalId => ExecutionData.GetInputParameter<string>(invln_updatesingleloanapplicationRequest.Fields.invln_contactexternalid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(loanApplication) && !string.IsNullOrEmpty(loanApplicationId) && !string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(contactExternalId);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().UpdateLoanApplication(loanApplicationId, loanApplication, fieldsToUpdate, accountId, contactExternalId);
        }

        #endregion
    }
}
