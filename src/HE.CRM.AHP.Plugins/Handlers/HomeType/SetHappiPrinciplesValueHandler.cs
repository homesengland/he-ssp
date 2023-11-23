using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.HomeType
{
    public class SetHappiPrinciplesValueHandler : CrmEntityHandlerBase<invln_HomeType, DataverseContext>
    {
        #region Fields

        private invln_HomeType target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IHomeTypeService>().SetHappiPrinciplesValue(target);
        }

        #endregion
    }
}
