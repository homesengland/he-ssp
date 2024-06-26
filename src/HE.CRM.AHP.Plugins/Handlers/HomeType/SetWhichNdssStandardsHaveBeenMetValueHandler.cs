using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.HomeType
{
    public class SetWhichNdssStandardsHaveBeenMetValueHandler : CrmEntityHandlerBase<invln_HomeType, DataverseContext>
    {
        #region Fields

        private invln_HomeType target => ExecutionData.Target;

        #endregion Fields

        #region Base Methods Overrides

        public override bool CanWork()
        {
            return ValueChanged(invln_HomeType.Fields.invln_whichndssstandardshavebeenmet);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IHomeTypeService>().SetWhichNdssStandardsHaveBeenMetValue(target);
        }

        #endregion Base Methods Overrides
    }
}
