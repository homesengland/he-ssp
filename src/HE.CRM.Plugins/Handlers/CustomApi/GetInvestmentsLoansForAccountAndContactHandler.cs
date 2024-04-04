using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;
using HE.CRM.Plugins.Services.LoanApplication;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetInvestmentsLoansForAccountAndContactHandler : CrmActionHandlerBase<invln_getloanapplicationsforaccountandcontactRequest, DataverseContext>
    {
        #region Fields

        private string accountId => ExecutionData.GetInputParameter<string>(invln_getloanapplicationsforaccountandcontactRequest.Fields.invln_accountid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getloanapplicationsforaccountandcontactRequest.Fields.invln_externalcontactid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getloanapplicationsforaccountandcontactRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(accountId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetInvestmentsLoansForAccountAndContact");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var loanApplications = CrmServicesFactory.Get<ILoanApplicationService>().GetLoanApplicationsForAccountAndContact(useHeTables, externalContactId, accountId);
            this.TracingService.Trace("Send Response");
            if (loanApplications != null)
            {
                ExecutionData.SetOutputParameter(invln_getloanapplicationsforaccountandcontactResponse.Fields.invln_loanapplications, loanApplications);
            }
        }

        #endregion
    }
}
