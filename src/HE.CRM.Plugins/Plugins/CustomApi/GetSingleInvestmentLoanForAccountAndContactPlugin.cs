using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getsingleloanapplicationforaccountandcontact",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetSingleInvestmentLoanForAccountAndContact: invln_getsingleloanapplicationforaccountandcontact",
    1,
    IsolationModeEnum.Sandbox,
    Id = "257390a9-1cc9-40af-977e-5bddf60fa55b")]
    public class GetSingleInvestmentLoanForAccountAndContactPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetSingleInvestmentLoanForAccountAndContactPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleInvestmentLoanForAccountAndContactHandler>());
        }
    }
}