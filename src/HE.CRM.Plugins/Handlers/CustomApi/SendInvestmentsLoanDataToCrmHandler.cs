using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using System.Text.Json;
using System.Web.Security;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class SendInvestmentsLoanDataToCrmHandler : CrmActionHandlerBase<invln_sendinvestmentloansdatatocrmRequest, DataverseContext>
    {
        #region Fields

        private string contactExternalId => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_contactexternalid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_accountid);
        private string loanApplicationId => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_loanapplicationid);
        private string requestStringMessage => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_entityfieldsparameters);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_sendinvestmentloansdatatocrmRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(contactExternalId) && !string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(requestStringMessage);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("SendInvestmentsLoanDataToCrmHandler");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var appId = CrmServicesFactory.Get<ILoanApplicationService>().CreateRecordFromPortal(useHeTables,contactExternalId, accountId, loanApplicationId, requestStringMessage);
            ExecutionData.SetOutputParameter(invln_sendinvestmentloansdatatocrmResponse.Fields.invln_loanapplicationid, appId);
        }

        #endregion
    }
}
