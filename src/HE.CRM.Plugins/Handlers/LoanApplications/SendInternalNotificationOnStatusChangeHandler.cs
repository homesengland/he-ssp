using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.LoanApplications
{
    public class SendInternalNotificationOnStatusChangeHandler : CrmEntityHandlerBase<invln_Loanapplication, DataverseContext>
    {
        #region Fields

        private invln_Loanapplication target => ExecutionData.Target;
        private invln_Loanapplication preImage => ExecutionData.PreImage;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null && preImage != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().SendInternalNotificationOnStatusChange(target, preImage);
        }

        #endregion
    }
}
