using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class CreateDocumentLocationOnCreateHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        #region Fields

        private invln_scheme target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null && target.invln_isallocation == false;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IApplicationService>().CreateDocumentLocation(target);
        }

        #endregion
    }
}
