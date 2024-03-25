using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.SiteDetails;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class UpdateSingleSiteDetailsHandler : CrmActionHandlerBase<invln_updatesinglesitedetailsRequest, DataverseContext>
    {
        #region Fields

        private string siteDetailsId => ExecutionData.GetInputParameter<string>(invln_updatesinglesitedetailsRequest.Fields.invln_sitedetailsid);
        private string siteDetail => ExecutionData.GetInputParameter<string>(invln_updatesinglesitedetailsRequest.Fields.invln_sitedetail);
        private string fieldsToUpdate => ExecutionData.GetInputParameter<string>(invln_updatesinglesitedetailsRequest.Fields.invln_fieldstoupdate);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_updatesinglesitedetailsRequest.Fields.invln_loanapplicationid);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_updatesinglesitedetailsRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(siteDetailsId) && !string.IsNullOrEmpty(siteDetail);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("UpdateSingleSiteDetailsHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            CrmServicesFactory.Get<ISiteDetailsService>().UpdateSiteDetails(useHeTables, siteDetailsId, siteDetail, fieldsToUpdate, loanApplicationId);
        }

        #endregion
    }
}
