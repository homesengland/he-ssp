using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using HE.CRM.Plugins.Services.SiteDetails;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class DeleteSiteDetailsHandler : CrmActionHandlerBase<invln_deletesitedetailsRequest, DataverseContext>
    {
        #region Fields

        private string siteDetailsId => ExecutionData.GetInputParameter<string>(invln_deletesitedetailsRequest.Fields.invln_sitedetailsid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return siteDetailsId != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ISiteDetailsService>().DeleteSiteDetails(siteDetailsId);
        }

        #endregion
    }
}
