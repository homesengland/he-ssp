using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using HE.Base.Plugins.Common.Constants;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class CheckIfApplicationWithNewNameExistsHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        #region Fields

        private invln_scheme target => ExecutionData.Target;
        private invln_scheme preImage => ExecutionData.Context.PreEntityImages.ContainsKey(nameof(ExecutionData.PreImage)) ? ExecutionData.PreImage : null;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null && target.invln_isallocation != true;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IApplicationService>().CheckIfApplicationWithNewNameExists(target, preImage);
        }

        #endregion
    }
}
