using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanStatusChange;

namespace HE.CRM.Plugins.Handlers.LoanStatusChange
{
    public class SendNotificationOnCreateHandler : CrmEntityHandlerBase<invln_Loanstatuschange, DataverseContext>
    {
        #region Fields

        private invln_Loanstatuschange target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanStatusChangeService>().SendNotificationOnCreate(target);
        }

        #endregion
    }
}
