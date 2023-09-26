using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
     "invln_sendinvestmentloansdatatocrm",
     "none",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.Plugins.Plugins.CustomApi.SendInvestmentsLoanDataToCrmPlugin: invln_sendinvestmentloansdatatocrm",
     1,
     IsolationModeEnum.Sandbox,
     Id = "AC1541B7-EA6A-46E1-A836-2C57C310CB33")]
    public class SendInvestmentsLoanDataToCrmPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Base Methods Overrides
        public SendInvestmentsLoanDataToCrmPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendInvestmentsLoanDataToCrmHandler>());
        }
        #endregion
    }
}
