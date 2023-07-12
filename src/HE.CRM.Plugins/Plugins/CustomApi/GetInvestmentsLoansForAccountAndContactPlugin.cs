using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getloanapplicationsforaccountandcontact",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetInvestmentsLoansForAccountAndContactPlugin: invln_getloanapplicationsforaccountandcontact",
    1,
    IsolationModeEnum.Sandbox,
    Id = "af53b65a-247f-47b4-8c3c-e1f85e851bc7")]
    public class GetInvestmentsLoansForAccountAndContactPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetInvestmentsLoansForAccountAndContactPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetInvestmentsLoansForAccountAndContactHandler>());
        }
    }
}