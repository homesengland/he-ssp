using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.SiteDetails;

namespace HE.CRM.Plugins.Handlers.SiteDetails
{
    public class FulfillRegionOnLocalAuthorityChangeHandler : CrmEntityHandlerBase<invln_SiteDetails, DataverseContext>
    {
        #region Fields

        private invln_SiteDetails target => ExecutionData.Target;
        private invln_SiteDetails preImage => ExecutionData.Context.PreEntityImages.ContainsKey(nameof(ExecutionData.PreImage)) ? ExecutionData.PreImage : null;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ISiteDetailsService>().FulfillRegionOnLocalAuthorityChange(target, preImage);
        }

        #endregion
    }
}
