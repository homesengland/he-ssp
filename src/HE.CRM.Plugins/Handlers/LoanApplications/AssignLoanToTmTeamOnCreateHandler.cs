using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.LoanApplications
{
    public class AssignLoanToTmTeamOnCreateHandler : CrmEntityHandlerBase<invln_Loanapplication, DataverseContext>
    {
        #region Fields

        private invln_Loanapplication target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().AssignLoanToTmTeam(target);
        }

        #endregion
    }
}
