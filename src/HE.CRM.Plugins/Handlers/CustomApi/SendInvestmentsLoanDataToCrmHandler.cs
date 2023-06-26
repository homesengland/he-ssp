using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    internal class SendInvestmentsLoanDataToCrmHandler : CrmActionHandlerBase<invln_sendinvestmentloansdatatocrmRequest, DataverseContext>
    {
        #region Fields

        private string requestStringMessage => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_entityfieldsparameters);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(requestStringMessage);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().CreateRecordFromPortal(requestStringMessage);
        }

        #endregion
    }
}
