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
     Id = "e4779b5a-c09a-4ed2-9a58-8b9bfdf394e7")]
    public class SendInvestmentsLoanDataToCrmPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public SendInvestmentsLoanDataToCrmPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendInvestmentsLoanDataToCrmHandler>());
        }
    }
}
