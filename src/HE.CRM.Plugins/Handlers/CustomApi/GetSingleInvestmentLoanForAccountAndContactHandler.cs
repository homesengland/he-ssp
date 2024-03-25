using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetSingleInvestmentLoanForAccountAndContactHandler : CrmActionHandlerBase<invln_getsingleloanapplicationforaccountandcontactRequest, DataverseContext>
    {
        #region Fields

        private string accountId => ExecutionData.GetInputParameter<string>(invln_getsingleloanapplicationforaccountandcontactRequest.Fields.invln_accountid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getsingleloanapplicationforaccountandcontactRequest.Fields.invln_externalcontactid);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_getsingleloanapplicationforaccountandcontactRequest.Fields.invln_loanapplicationid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsingleloanapplicationforaccountandcontactRequest.Fields.invln_fieldstoretrieve);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsingleloanapplicationforaccountandcontactRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(loanApplicationId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleInvestmentLoanForAccountAndContact");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var loanApplication = CrmServicesFactory.Get<ILoanApplicationService>().GetLoanApplicationsForAccountAndContact(useHeTables, externalContactId, accountId, loanApplicationId, fieldsToRetrieve);
            this.TracingService.Trace("Send Response");
            if (loanApplication != null)
            {
                ExecutionData.SetOutputParameter(invln_getsingleloanapplicationforaccountandcontactResponse.Fields.invln_loanapplication, loanApplication);
            }
        }

        #endregion
    }
}
