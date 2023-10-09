using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetFileLocationForApplicationLoanHandler : CrmActionHandlerBase<invln_getfilelocationforapplicationloanRequest, DataverseContext>
    {
        #region Fields

        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_getfilelocationforapplicationloanRequest.Fields.invln_loanapplicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(loanApplicationId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetFileLocationForApplicationLoan");
            var fileLocation = CrmServicesFactory.Get<ILoanApplicationService>().GetFileLocationForApplicationLoan(loanApplicationId);
            if (fileLocation != null)
            {
                ExecutionData.SetOutputParameter(invln_getfilelocationforapplicationloanResponse.Fields.invln_filelocation, fileLocation);
            }
        }

        #endregion
    }
}
