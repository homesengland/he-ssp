using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetOrganisationChangeDetailsHandler : CrmActionHandlerBase<invln_getorganisationchangedetailsRequest, DataverseContext>
    {
        #region Fields

        private string accountId => ExecutionData.GetInputParameter<string>(invln_getorganisationchangedetailsRequest.Fields.invln_accountid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(accountId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetInvestmentsLoansForAccountAndContact");
            var whoRequested = CrmServicesFactory.Get<IAccountService>().GetOrganisationChangeDetails(accountId);
            this.TracingService.Trace("Send Response");
            if (whoRequested != null)
            {
                ExecutionData.SetOutputParameter(invln_getorganisationchangedetailsResponse.Fields.invln_whorequested, whoRequested);
            }
        }

        #endregion
    }
}
