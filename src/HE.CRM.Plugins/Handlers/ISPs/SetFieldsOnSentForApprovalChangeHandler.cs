using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.ISPs;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.ISPs
{
    public class SetFieldsOnSentForApprovalChangeHandler : CrmEntityHandlerBase<invln_ISP, DataverseContext>
    {
        #region Fields

        private invln_ISP target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IIspService>().SetFieldsOnSentForApprovalChange(target);
        }

        #endregion
    }
}
