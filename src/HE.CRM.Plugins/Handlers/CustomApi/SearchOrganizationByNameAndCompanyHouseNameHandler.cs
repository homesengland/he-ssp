using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class SearchOrganizationByNameAndCompanyHouseNameHandler : CrmActionHandlerBase<invln_searchorganizationbynameandcompanyhousenameRequest, DataverseContext>
    {
        #region Fields

        private string organizationName => ExecutionData.GetInputParameter<string>(invln_searchorganizationbynameandcompanyhousenameRequest.Fields.invln_organizationname);
        private string companyHouseNumber => ExecutionData.GetInputParameter<string>(invln_searchorganizationbynameandcompanyhousenameRequest.Fields.invln_companyhousenumber);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(companyHouseNumber) || !string.IsNullOrEmpty(organizationName);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleInvestmentLoanForAccountAndContact");
            var organizations = CrmServicesFactory.Get<IAccountService>().SearchOrganizationByNameAndCompanyHouseNumber(organizationName, companyHouseNumber);
            this.TracingService.Trace("Send Response");
            if (organizations != null)
            {
                ExecutionData.SetOutputParameter(invln_searchorganizationbynameandcompanyhousenameResponse.Fields.invln_organization, JsonSerializer.Serialize(organizations));
            }
        }

        #endregion
    }
}
