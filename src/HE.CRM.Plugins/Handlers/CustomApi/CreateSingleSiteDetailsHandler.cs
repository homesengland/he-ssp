using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using HE.CRM.Plugins.Services.SiteDetails;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class CreateSingleSiteDetailsHandler : CrmActionHandlerBase<invln_createsinglesitedetailRequest, DataverseContext>
    {
        #region Fields

        private string siteDetails => ExecutionData.GetInputParameter<string>(invln_createsinglesitedetailRequest.Fields.invln_sitedetails);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_createsinglesitedetailRequest.Fields.invln_loanapplicationid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_createsinglesitedetailRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return siteDetails != null && !string.IsNullOrEmpty(loanApplicationId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("CreateSingleSiteDetailsHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            CrmServicesFactory.Get<ISiteDetailsService>().CreateSiteDetail(useHeTables, siteDetails, loanApplicationId);
        }

        #endregion
    }
}
