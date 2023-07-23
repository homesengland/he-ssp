using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetOrganizationDetailsHandler : CrmActionHandlerBase<invln_getorganizationdetailsRequest, DataverseContext>
    {
        #region Fields

        private string accountId => ExecutionData.GetInputParameter<string>(invln_getorganizationdetailsRequest.Fields.invln_accountid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getorganizationdetailsRequest.Fields.invln_contactexternalid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(accountId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetInvestmentsLoansForAccountAndContact");
            var organizationDetails = CrmServicesFactory.Get<IAccountService>().GetOrganizationDetails(accountId, externalContactId);
            this.TracingService.Trace("Send Response");
            if (organizationDetails != null)
            {
                ExecutionData.SetOutputParameter(invln_getorganizationdetailsResponse.Fields.invln_organizationdetails, JsonSerializer.Serialize(organizationDetails));
            }
        }

        #endregion
    }
}
