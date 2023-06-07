using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Xrm.Sdk;
using System;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    internal class SendInvestmentsLoanDataToCrmHandler : CrmActionHandlerBase<invln_sendloansinvestmentdatatocrmRequest, DataverseContext>
    {
        #region Fields

        private string requestStringMessage => ExecutionData.GetInputParameter<string>(invln_sendloansinvestmentdatatocrmRequest.Fields.invln_entityparameters);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(requestStringMessage);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<ILoanApplicationService>().CreateRecordFromPortal(requestStringMessage);
            //ExecutionData.SetOutputParameter<string>(crba8_testcustomapiResponse.Fields.crba8_testresponse, testResult);
        }

        #endregion
    }
}
