using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class SearchOrganizationByNameAndCompanyHouseNameHandler : CrmActionHandlerBase<invln_searchorganizationbynameandcompanyhousenameRequest, DataverseContext>
    {
        #region Fields

        private string organizationName => ExecutionData.GetInputParameter<string>(invln_searchorganizationbynameandcompanyhousenameRequest.Fields.invln_organizationname);
        private string companyHouseName => ExecutionData.GetInputParameter<string>(invln_searchorganizationbynameandcompanyhousenameRequest.Fields.invln_companyhousename);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(companyHouseName) && !string.IsNullOrEmpty(organizationName);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleInvestmentLoanForAccountAndContact");
            var loanApplication = CrmServicesFactory.Get<IAccountService>().GetLoanApplicationsForAccountAndContact(companyHouseName, organizationName);
            this.TracingService.Trace("Send Response");
            if (loanApplication != null)
            {
                ExecutionData.SetOutputParameter(invln_searchorganizationbynameandcompanyhousenameResponse.Fields.invln_organization, loanApplication);
            }
        }

        #endregion
    }
}
