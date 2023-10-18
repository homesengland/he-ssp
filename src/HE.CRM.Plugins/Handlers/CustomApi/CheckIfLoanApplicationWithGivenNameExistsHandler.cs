using System.Text.Json;
using System.Web.Security;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class CheckIfLoanApplicationWithGivenNameExistsHandler : CrmActionHandlerBase<invln_checkifloanapplicationwithgivennameexistsRequest, DataverseContext>
    {
        #region Fields

        private string loanName => ExecutionData.GetInputParameter<string>(invln_checkifloanapplicationwithgivennameexistsRequest.Fields.invln_loanname);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_checkifloanapplicationwithgivennameexistsRequest.Fields.invln_organisationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return loanName != null;
        }

        public override void DoWork()
        {
            var loanExists = CrmServicesFactory.Get<ILoanApplicationService>().CheckIfLoanApplicationWithGivenNameExists(loanName, organisationId);
            ExecutionData.SetOutputParameter(invln_checkifloanapplicationwithgivennameexistsResponse.Fields.invln_loanexists, loanExists);
        }

        #endregion
    }
}
