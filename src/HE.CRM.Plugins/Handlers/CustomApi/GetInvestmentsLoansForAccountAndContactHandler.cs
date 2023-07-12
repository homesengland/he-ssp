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

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(accountId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetInvestmentsLoansForAccountAndContact");
            var loanApplications = CrmServicesFactory.Get<ILoanApplicationService>().GetLoanApplicationsForAccountAndContact(externalContactId, accountId);
            this.TracingService.Trace("Send Response");
            if (loanApplications != null)
            {
                ExecutionData.SetOutputParameter(invln_getloanapplicationsforaccountandcontactResponse.Fields.invln_loanapplications, loanApplications);
            }
        }

        #endregion
    }
}